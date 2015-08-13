using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emdat.Threading;
using System.Diagnostics;
using System.Threading;
using Emdat.Diagnostics;
using System.Net;

namespace InVision.SchedulingService
{
    internal class ReportDispatcher : WorkerThreadBase
    {
        #region throttling

        /// <summary>
        /// Gets or sets the polling interval.
        /// </summary>
        /// <value>The polling interval.</value>
        public TimeSpan PollingInterval
        {
            get
            {
                return _pollingInterval;
            }
            set
            {
                if (value <= TimeSpan.Zero)
                {
                    throw new ArgumentOutOfRangeException("PollingInterval must be greater than Zero");
                }
                _pollingInterval = value;
            }
        }
        private TimeSpan _pollingInterval = TimeSpan.FromSeconds(60);
        private bool _waitingForPollingInterval = false;

        /// <summary>
        /// Gets or sets the max concurrent requests.
        /// </summary>
        /// <value>The max concurrent requests.</value>
        public int MaxConcurrentExecutions
        {
            get
            {
                return _maxConcurrentExecutions;
            }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("MaxConcurrentRequests must be greater than Zero");
                }
                _maxConcurrentExecutions = value;
            }
        }
        private int _maxConcurrentExecutions = 10;

        #endregion

        #region recovery

        /// <summary>
        /// Gets or sets the recovery interval.
        /// </summary>
        /// <value>The recovery interval.</value>
        public TimeSpan InitialRecoveryInterval
        {
            get
            {
                return _initialRecoveryInterval;
            }
            set
            {
                if (value < TimeSpan.Zero)
                {
                    throw new ArgumentException("InitialRecoveryInterval must not be less than TimeSpan.Zero");
                }
                _initialRecoveryInterval = value;
            }
        }
        private TimeSpan _initialRecoveryInterval = TimeSpan.FromSeconds(5);
        private bool _recoverySleep = false;
        private const int MAX_RECOVERY_INTERVAL_IN_MILLISECONDS = 300000; //15 minutes

        /// <summary>
        /// Gets or sets the max consecutive failures.
        /// </summary>
        /// <value>The max consecutive failures.</value>
        public int MaxConsecutiveFailures
        {
            get
            {
                return _maxConsecutiveFailures;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("MaxConsecutiveFailures");
                }
                _maxConsecutiveFailures = value;
            }
        }
        private int _maxConsecutiveFailures = 5;
        private int _consecutiveFailures = 0;

        /// <summary>
        /// Gets or sets the current recovery interval.
        /// </summary>
        /// <value>The current recovery interval.</value>
        private TimeSpan CurrentRecoveryInterval
        {
            get
            {
                return _currentRecoveryInterval;
            }
            set
            {
                _currentRecoveryInterval = (value < TimeSpan.FromMilliseconds(MAX_RECOVERY_INTERVAL_IN_MILLISECONDS) ? value : TimeSpan.FromMilliseconds(MAX_RECOVERY_INTERVAL_IN_MILLISECONDS));
            }
        }
        private TimeSpan _currentRecoveryInterval = TimeSpan.FromSeconds(5);

        #endregion

        #region execution settings

        /// <summary>
        /// Gets or sets the execution timeout.
        /// </summary>
        /// <value>The execution timeout.</value>
        public TimeSpan ExecutionTimeout
        {
            get
            {
                return _executionTimeout;
            }
            set
            {
                if (value <= TimeSpan.Zero)
                {
                    throw new ArgumentException("ExecutionTimeout must be greater than zero", "value");
                }
                _executionTimeout = value;
            }
        }
        private TimeSpan _executionTimeout = TimeSpan.FromMinutes(30);

        /// <summary>
        /// Gets or sets a value indicating whether [use default execution credentials].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [use default execution credentials]; otherwise, <c>false</c>.
        /// </value>
        public bool UseDefaultExecutionCredentials
        {
            get { return _useDefaultReportServerCredentials; }
            set { _useDefaultReportServerCredentials = value; }
        }
        private bool _useDefaultReportServerCredentials = true;

        /// <summary>
        /// Gets or sets the execution credentials.
        /// </summary>
        /// <value>The execution credentials.</value>
        public ICredentials ExecutionCredentials { get; set; }

        #endregion

        /// <summary>
        /// Does the work.
        /// </summary>
        protected override void DoWork()
        {
            #region validate configuration

            Debug.Assert(this.MaxConcurrentExecutions > 0);

            //TODO: validate configuration            

            #endregion

            //use a semaphore to throttle the number of actively processing requests
            using (Semaphore semaphore = new Semaphore(MaxConcurrentExecutions, MaxConcurrentExecutions))
            {
                //need to wait on the semaphore and the wake event
                WaitHandle[] waitHandles = new WaitHandle[]
                {
                    semaphore,
                    base._wakeEvent
                };

                //start the main loop
                while (this.State == WorkerThreadState.Running)
                {
                    try
                    {

                        #region recovery

                        if (_recoverySleep)
                        {
                            Logger.TraceEvent(TraceEventType.Verbose, 0, "{0}: Waiting for recovery interval: {1}", this.Name, this.CurrentRecoveryInterval);
                            this.Sleep(this.CurrentRecoveryInterval);
                            _recoverySleep = false;
                            this.CurrentRecoveryInterval = this.CurrentRecoveryInterval.Add(this.CurrentRecoveryInterval);
                            if (this.State != WorkerThreadState.Running)
                            {
                                return;
                            }
                        }

                        #endregion

                        #region throttling

                        //implement throttling to control the number of concurrent work items queued on the Thread Pool
                        Logger.TraceEvent(TraceEventType.Verbose, 0, "{0}: Waiting on semaphore.", this.Name);
                        int eventIndex = WaitHandle.WaitAny(waitHandles);
                        if (this.State != WorkerThreadState.Running)
                        {
                            return;
                        }

                        #endregion

                        //get next report execution
                        ReportExecution job = null;
                        try
                        {
                            Logger.TraceEvent(TraceEventType.Information, 0, "{0}: Getting next scheduled report execution.", this.Name);
                            job = ReportExecution.GetNextJob();
                        }
                        catch
                        {
                            //on error, release semaphore
                            Logger.TraceEvent(TraceEventType.Verbose, 0, "{0}: Error getting report execution. Releasing semaphore.", this.Name);
                            semaphore.Release();
                            throw;
                        }

                        if (job == null)
                        {
                            //on no job, release semaphore and go to sleep
                            Logger.TraceEvent(TraceEventType.Information, 0, "{0}: No queued report executions.", this.Name);
                            Logger.TraceEvent(TraceEventType.Verbose, 0, "{0}: Releasing semaphore.", this.Name);
                            semaphore.Release();

                            _waitingForPollingInterval = true;
                            this.Sleep(this.PollingInterval);
                            _waitingForPollingInterval = false;
                        }
                        else
                        {
                            //handle message on ThreadPool                                
                            ThreadPool.QueueUserWorkItem(state =>
                            {
                                #region perform execution

                                try
                                {
                                    try
                                    {
                                        #region execute the report

                                        using (AutoResetEvent renderedEvent = new AutoResetEvent(false))
                                        {                                                                                      
                                            job.RenderCompleted += delegate(object sender, ReportExecutionEventArgs e)
                                            {
                                                #region handle render completed event
                                                
                                                Logger.TraceEvent(TraceEventType.Verbose, 0, "{0}: BEGIN RenderCompleted", e.Job.Id);
                                                try
                                                {
                                                    if (renderedEvent != null &&
                                                        !renderedEvent.SafeWaitHandle.IsClosed &&
                                                        !renderedEvent.SafeWaitHandle.IsInvalid)
                                                    {
                                                        renderedEvent.Set();
                                                    }
                                                }
                                                finally
                                                {
                                                    Logger.TraceEvent(TraceEventType.Verbose, 0, "{0}: END RenderCompleted", e.Job.Id);
                                                }

                                                #endregion
                                            };
                                            job.ExecuteCompleted += new EventHandler<ReportExecutionEventArgs>(job_ExecuteCompleted);                                            

                                            //execute the report
                                            Logger.TraceEvent(TraceEventType.Information, 0, "{0}: Processing report execution {1}.", this.Name, job.Id);
                                            //job.Execute(this.ExecutionCredentials, this.ExecutionTimeout);  
                                            var format = job.Format;
                                            job.ExecuteAsync(this.ExecutionCredentials, this.ExecutionTimeout, format);
                                            if (!renderedEvent.WaitOne(this.ExecutionTimeout, true))
                                            {
                                                Logger.TraceEvent(TraceEventType.Warning, 0, "{0}: Rendering timed out.", job.Id);
                                                
                                                //cancel the execution to ensure that the completed event never fires
                                                job.CancelExecute();

                                                job.Error = new TimeoutException("The operation timed out waiting for rendering");
                                                job.ErrorCode = ReportExecutionErrorEnum.ExecutionTimeout;
                                                job.ErrorCount++;
                                                job.EndTime = DateTime.Now;
                                                job.State = ReportExecutionStateEnum.Failed;

												TraceEventType eventType;

												switch (job.ErrorCode)
												{
                                                    case ReportExecutionErrorEnum.InvalidReportParameter:
                                                    {
                                                        eventType = TraceEventType.Information;
                                                        break;
                                                    }
                                                    default:
                                                    {
                                                        eventType = TraceEventType.Error;
                                                        break;
                                                    }
												}

                                                Logger.TraceEvent(
													eventType,
                                                    0,
                                                    "{0}: Report execution {1} failed. {2}.{3}{4}",
                                                    this.Name,
                                                    job.Id,
                                                    job.Error != null ? job.Error.Message : string.Empty,
                                                    Environment.NewLine,
                                                    job.Error != null ? job.Error.ToString() : string.Empty);

                                                //complete the execution 
                                                Logger.TraceEvent(TraceEventType.Information, 0, "{0}: Completing report execution {1}.", this.Name, job.Id);
                                                ReportExecution.CompleteExecution(job);
                                            }
                                            
                                            //At this point, the report has rendered and is downloading, so we can release 
                                            //the semaphore to let another execution through. The ExecuteCompleted event 
                                            //will fire when the report has finished downloading.
                                        }

                                        #endregion
                                    }
                                    catch (Exception ex)
                                    {
                                        //TODO: more detailed exception handling
                                        Logger.TraceEvent(TraceEventType.Error, 0, "{0}: {1}", ex.GetType(), ex.Message);
                                        Logger.TraceEvent(TraceEventType.Information, 0, ex.ToString());
                                        _recoverySleep = true;
                                    }
                                }
                                finally
                                {
                                    if (!semaphore.SafeWaitHandle.IsClosed)
                                    {
                                        Logger.TraceEvent(TraceEventType.Verbose, 0, "{0}: Releasing semaphore.", this.Name);
                                        semaphore.Release();
                                    }

                                    //TODO: review the following code. It was indirectly causing a SemaphoreFullException.
                                    ////wake-up the dispatcher thread if it is not in "recovery mode"
                                    ////this will prevent unnecessary waiting if the next execution time 
                                    ////of the completed execution is already past
                                    //if (!_recoverySleep && _waitingForPollingInterval)
                                    //{
                                    //    this.WakeUp();
                                    //}
                                }

                                #endregion
                            });
                        }
                    }

                    #region exception handling

                    catch (Exception ex)
                    {
                        //TODO: only handle specific exceptions
                        Logger.TraceEvent(TraceEventType.Error, 0, "{0}: {1}", ex.GetType(), ex.Message);
                        Logger.TraceEvent(TraceEventType.Information, 0, ex.ToString());
                        _recoverySleep = true;
                    }

                    #endregion
                }
            }
        }

        private void job_ExecuteCompleted(object sender, ReportExecutionEventArgs e)
        {
            #region logging

            Logger.TraceEvent(TraceEventType.Verbose, 0, "{0}: BEGIN ExecuteCompleted", e.Job.Id);

            #endregion

            try
            {
                try
                {
                    try
                    {
                        if (e.Job.State == ReportExecutionStateEnum.Failed)
                        {
                            //only "unexpected" errors should trigger recovery action
                            if (e.Job.ErrorCode.GetValueOrDefault(ReportExecutionErrorEnum.None) == ReportExecutionErrorEnum.None)                                
                            {
                                Interlocked.Increment(ref _consecutiveFailures);
                            }
                            Logger.TraceEvent(
                                TraceEventType.Error,
                                0,
                                "{0}: Report execution {1} failed. {2}.{3}{4}",
                                this.Name,
                                e.Job.Id,
                                e.Job.Error != null ? e.Job.Error.Message : string.Empty,
                                Environment.NewLine,
                                e.Job.Error != null ? e.Job.Error.ToString() : string.Empty);
                            if (_consecutiveFailures > this.MaxConsecutiveFailures)
                            {
                                _recoverySleep = true;
                            }
                        }
                        else
                        {
                            _consecutiveFailures = 0;
							this.CurrentRecoveryInterval = TimeSpan.FromSeconds(5);
							_recoverySleep = false;
                        }
                    }
                    finally
                    {
                        //complete the execution 
                        Logger.TraceEvent(TraceEventType.Information, 0, "{0}: Completing report execution {1}.", this.Name, e.Job.Id);
                        ReportExecution.CompleteExecution(e.Job);
                    }
                }

                #region exception handling

                catch (Exception ex)
                {
                    //TODO: more detailed exception handling
                    Logger.TraceEvent(TraceEventType.Error, 0, "{0}: {1}", ex.GetType(), ex.Message);
                    Logger.TraceEvent(TraceEventType.Information, 0, ex.ToString());
                    _recoverySleep = true;
                }

                #endregion
            }

            #region logging

            finally
            {
                Logger.TraceEvent(TraceEventType.Verbose, 0, "{0}: END ExecuteCompleted", e.Job.Id);
            }

            #endregion
        }        
    }
}
