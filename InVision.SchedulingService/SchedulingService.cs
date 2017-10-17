using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using InVision.SchedulingService.Properties;
using Emdat.Diagnostics;
using System.Net;
using System.Timers;

namespace InVision.SchedulingService
{
    /// <summary>
    /// Implements the Windows Service.
    /// </summary>
    /// <remarks>
    /// As a best practice, in keeping with the single responsiblity principle, 
    /// the InVision.SchedulingService class should be implemented as a wrapper for the 
    /// other threads that do the real work. In other words, very little
    /// application specific logic should be implemented here.
    /// </remarks>
    public partial class SchedulingService : ServiceBase
    {
        private ReportDispatcher _dispatcher;
        private Timer _aliveTimer;

        /// <summary>
        /// Initializes a new instance of the <see cref="InVision.SchedulingService"/> class.
        /// </summary>
        public SchedulingService()
        {
            InitializeComponent();

            _aliveTimer = new Timer();
            _aliveTimer.Interval = 60000;
            _aliveTimer.Enabled = false;
            _aliveTimer.Elapsed += new ElapsedEventHandler(_aliveTimer_Elapsed);

            //setting the service name at runtime will allow multiple instances
            //of the service to be installed
            this.ServiceName = Settings.Default.ServiceName;
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Start command is sent to the service by the Service Control Manager (SCM) or when the operating system starts (for a service that starts automatically). Specifies actions to take when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected override void OnStart(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(OnUnhandledException);

			//reset running jobs
			ReportExecution.MaxReportFailures = Settings.Default.MaxReportFailures;
			ReportExecution.ResetRunningJobs();

            //start worker threads, timers, etc. here
            _dispatcher = new ReportDispatcher();
            _dispatcher.InitialRecoveryInterval = Settings.Default.InitialRecoveryInterval;
            _dispatcher.MaxConcurrentExecutions = Settings.Default.MaxConcurrentExecutions;
            _dispatcher.MaxConsecutiveFailures = Settings.Default.MaxConsecutiveFailures;
            _dispatcher.Name = "SCHEDULER";
            _dispatcher.PollingInterval = Settings.Default.PollingInterval;
            _dispatcher.ExecutionTimeout = Settings.Default.ReportServerExecutionTimeout;
			
            //HACK: To get around issues between DEV-DB02 and the DC, use a local user
            if (0 == string.Compare(System.Configuration.ConfigurationManager.AppSettings["ReportServerUseDefaultCredentials"], "FALSE", StringComparison.OrdinalIgnoreCase))
            {
                if (String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["ReportServerUserName"]))
                {
                    throw new InvalidOperationException("ReportServerUserName must be specified when ReportServerUseDefaultCredentials is false");
                }
                if (String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["ReportServerPassword"]))
                {
                    throw new InvalidOperationException("ReportServerPassword must be specified when ReportServerUseDefaultCredentials is false");
                }
                _dispatcher.ExecutionCredentials = new NetworkCredential(
                    System.Configuration.ConfigurationManager.AppSettings["ReportServerUserName"],
                    System.Configuration.ConfigurationManager.AppSettings["ReportServerPassword"],
                    System.Configuration.ConfigurationManager.AppSettings["ReportServerDomain"]);

            }
            else
            {
                _dispatcher.ExecutionCredentials = null;
            }            
            _dispatcher.Start();

            if (Settings.Default.ReportServerKeepAliveEnabled)
            {
                //start the alive timer
                _aliveTimer.Start();
            }
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Stop command is sent to the service by the Service Control Manager (SCM). Specifies actions to take when a service stops running.
        /// </summary>
        protected override void OnStop()
        {
            //clean-up worker threads, timers, etc. here
            if (_dispatcher != null)
            {
                _dispatcher.Stop(TimeSpan.FromSeconds(30));
            }

            //stop the alive timer
            _aliveTimer.Stop();

            //reset running jobs
            ReportExecution.ResetRunningJobs();
        }

        private void _aliveTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            #region logging

            Logger.TraceEvent(TraceEventType.Verbose, 0, "{0}: KEEPALIVE: BEGIN _aliveTimer_Elapsed(sender={1}, e.SignalTime={2})", System.Threading.Thread.CurrentThread.ManagedThreadId, sender, e.SignalTime);

            #endregion

            try
            {
                try
                {
                    //stop the timer
                    _aliveTimer.Stop();

                    #region ping report execution service to prevent it from winding down

                    using (Emdat.InVision.SSRSExecution.ReportExecutionService ssrsExec = new Emdat.InVision.SSRSExecution.ReportExecutionService())
                    {
                        //HACK: To get around issues between DEV-DB02 and the DC, use a local user
                        if (0 == string.Compare(System.Configuration.ConfigurationManager.AppSettings["ReportServerUseDefaultCredentials"], "FALSE", StringComparison.OrdinalIgnoreCase))
                        {
                            ssrsExec.UseDefaultCredentials = false;
                            ssrsExec.Credentials = new NetworkCredential(
                                System.Configuration.ConfigurationManager.AppSettings["ReportServerUserName"],
                                System.Configuration.ConfigurationManager.AppSettings["ReportServerPassword"],
                                System.Configuration.ConfigurationManager.AppSettings["ReportServerDomain"]);
                        }
                        else
                        {
                            ssrsExec.UseDefaultCredentials = true;
                        }

                        try
                        {
                            Logger.TraceEvent(TraceEventType.Verbose, 0, "{0}: KEEPALIVE: Calling ReportExecutionService.LoadReport(Report={1}, HistoryId={2}).", System.Threading.Thread.CurrentThread.ManagedThreadId, Settings.Default.ReportServerTestReport, null);
                            var execInfo = ssrsExec.LoadReport(Settings.Default.ReportServerTestReport, null);
                            Logger.TraceEvent(TraceEventType.Verbose, 0, "{0}: KEEPALIVE: ReportExecutionService.LoadReport() returned {1}.", System.Threading.Thread.CurrentThread.ManagedThreadId, execInfo);

                            Logger.TraceEvent(TraceEventType.Verbose, 0, "{0}: KEEPALIVE: Calling ReportExecutionService.ResetExecution().", System.Threading.Thread.CurrentThread.ManagedThreadId);
                            execInfo = ssrsExec.ResetExecution();
                            Logger.TraceEvent(TraceEventType.Verbose, 0, "{0}: KEEPALIVE: ReportExecutionService.ResetExecution() returned {1}.", System.Threading.Thread.CurrentThread.ManagedThreadId, execInfo);

                        }
                        catch (Exception ex)
                        {
                            Logger.TraceEvent(TraceEventType.Information, 0, "KEEPALIVE: An exception occurred when pinging the ReportExecutionService. {0}. See the service logs for more detail.", ex.Message);
                            throw;
                        }
                    }

                    #endregion

                    #region ping reporting service to prevent it from winding down

                    using (Emdat.InVision.SSRS.ReportingService2005 ssrs = new Emdat.InVision.SSRS.ReportingService2005())
                    {
                        //HACK: To get around issues between DEV-DB02 and the DC, use a local user
                        if (0 == string.Compare(System.Configuration.ConfigurationManager.AppSettings["ReportServerUseDefaultCredentials"], "FALSE", StringComparison.OrdinalIgnoreCase))
                        {
                            ssrs.UseDefaultCredentials = false;
                            ssrs.Credentials = new NetworkCredential(
                                System.Configuration.ConfigurationManager.AppSettings["ReportServerUserName"],
                                System.Configuration.ConfigurationManager.AppSettings["ReportServerPassword"],
                                System.Configuration.ConfigurationManager.AppSettings["ReportServerDomain"]);
                        }
                        else
                        {
                            ssrs.UseDefaultCredentials = true;
                        }

                        try
                        {
                            Logger.TraceEvent(TraceEventType.Verbose, 0, "{0}: KEEPALIVE: Calling ReportingService2005.ListReportHistory({1}).", System.Threading.Thread.CurrentThread.ManagedThreadId, Settings.Default.ReportServerTestReport);
                            var historyItems = ssrs.ListReportHistory(Settings.Default.ReportServerTestReport);
                            Logger.TraceEvent(TraceEventType.Verbose, 0, "{0}: KEEPALIVE: ReportingService2005.ListReportHistory() returned {1} items.", System.Threading.Thread.CurrentThread.ManagedThreadId, (historyItems != null ? historyItems.Length : 0));
                        }
                        catch (Exception ex)
                        {
                            Logger.TraceEvent(TraceEventType.Information, 0, "{0}: KEEPALIVE: An exception occurred when pinging the ReportingService2005. {1}. See the service logs for more detail.", System.Threading.Thread.CurrentThread.ManagedThreadId, ex.Message);
                            throw;
                        }
                    }

                    #endregion

                    //pings were successful, so reset the timer interval
                    _aliveTimer.Interval = 60000;
                }
                catch (Exception ex)
                {
                    //log EMDATERROR
                    Logger.TraceEvent(TraceEventType.Error, 0, "{0}: KEEPALIVE: An exception occurred when pinging the report server. {1}. See the service logs for more detail.", System.Threading.Thread.CurrentThread.ManagedThreadId, ex.Message);
                    Logger.TraceEvent(TraceEventType.Information, 0, "{0}: KEEPALIVE: {1}", System.Threading.Thread.CurrentThread.ManagedThreadId, ex.ToString());

                    //add 1 minute interval (up to 15 minutes) to prevent spamming EMDATERROR
                    _aliveTimer.Interval = Math.Max(_aliveTimer.Interval + 60000, TimeSpan.FromMinutes(15).TotalMilliseconds);
                }
                finally
                {
                    //restart the timer
                    _aliveTimer.Start();
                }
            }

            #region logging

            finally
            {
                Logger.TraceEvent(TraceEventType.Verbose, 0, "{0}: KEEPALIVE: END _aliveTimer_Elapsed", System.Threading.Thread.CurrentThread.ManagedThreadId);
            }

            #endregion
        }

        /// <summary>
        /// Called when an unhandled exception occurs.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.UnhandledExceptionEventArgs"/> instance containing the event data.</param>
        /// <remarks>
        /// The application will still crash, but this method provides an opportunity to log more detailed information about the event.
        /// </remarks>
        protected virtual void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                try
                {
                    Logger.TraceEvent(TraceEventType.Critical, 0, ex.ToString());
                }
                catch
                {
                    this.EventLog.WriteEntry(ex.ToString(), EventLogEntryType.Error);
                }
            }
        }

        #region helper methods for running as a console application

        /// <summary>
        /// Starts the service.
        /// </summary>
        /// <param name="args">The args.</param>
        internal void StartService(string[] args)
        {
            this.OnStart(args);
        }

        /// <summary>
        /// Stops the service.
        /// </summary>
        internal void StopService()
        {
            this.OnStop();
        }

        #endregion
    }
}
