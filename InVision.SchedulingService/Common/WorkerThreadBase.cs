using System;
using System.Diagnostics;
using System.Threading;

namespace Emdat.Threading
{
    /// <summary>
    /// WorkerThreadState
    /// </summary>
    [Serializable]
    public enum WorkerThreadState
    {
        /// <summary>
        /// Uninitialized
        /// </summary>
        Uninitialized,
        /// <summary>
        /// Starting
        /// </summary>
        Starting,
        /// <summary>
        /// Running
        /// </summary>
        Running,
        /// <summary>
        /// Terminating
        /// </summary>
        Terminating,
        /// <summary>
        /// Terminated
        /// </summary>
        Terminated
    }

    /// <summary>
    /// WorkerThread
    /// </summary>
    [Serializable]
    public abstract class WorkerThreadBase : MarshalByRefObject, IDisposable
    {
        /// <summary>
        /// the thread
        /// </summary>
        protected Thread _workerThread;

        /// <summary>
        /// the wake event
        /// </summary>
        protected AutoResetEvent _wakeEvent = new AutoResetEvent(false);

        private bool _disposed = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is background thread.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is background thread; otherwise, <c>false</c>.
        /// </value>
        public bool IsBackgroundThread { get; set; }

        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <value>The state.</value>
        public WorkerThreadState State { get; private set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Starts receiving jobs.
        /// </summary>
        public void Start()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(null);
            }

            if (State == WorkerThreadState.Uninitialized ||
                State == WorkerThreadState.Terminated)
            {
                State = WorkerThreadState.Starting;

                OnStarting();

                //start the worker thread
                _workerThread = new Thread(new ThreadStart(Execute));
                _workerThread.IsBackground = this.IsBackgroundThread;
                _workerThread.Start();
            }
            else
            {
                throw new InvalidOperationException(string.Format("Thread cannot be started from current state: {0}", State));
            }
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            this.Stop(TimeSpan.FromSeconds(60));
        }

        /// <summary>
        /// Stops the thread
        /// </summary>
        /// <param name="timeout">The timeout. Specify TimeSpan.Zero to stop immeidately.</param>
        public void Stop(TimeSpan timeout)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(null);
            }

            if (_workerThread != null &&
                State != WorkerThreadState.Terminating &&
                State != WorkerThreadState.Terminated)
            {
                State = WorkerThreadState.Terminating;

                OnTerminating();

                //make sure the thread stops
                this.WakeUp();
                _workerThread.Interrupt();
                if (timeout != TimeSpan.Zero &&
                    _workerThread.ThreadState != System.Threading.ThreadState.Unstarted)
                {
                    if (!_workerThread.Join(timeout))
                    {
                        _workerThread.Abort();
                    }
                }
            }
        }

        /// <summary>
        /// Sleeps for the specified timeout.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        protected void Sleep(TimeSpan timeout)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(null);
            }

            _wakeEvent.WaitOne(timeout, true);
        }

        /// <summary>
        /// Wakes up.
        /// </summary>
        protected void WakeUp()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(null);
            }

            _wakeEvent.Set();
        }

        /// <summary>
        /// Executes the thread
        /// </summary>
        private void Execute()
        {
            try
            {
                //mark the thread as started
                State = WorkerThreadState.Running;

                //do work
                DoWork();
            }
            catch (ThreadInterruptedException)
            {
                Trace.TraceInformation("{0}: ThreadInterruptedException caught.", Name);
            }
            catch (ThreadAbortException)
            {
                Trace.TraceInformation("{0}: ThreadAbortException caught.", Name);
                Thread.ResetAbort();
            }
            catch (Exception ex)
            {
                Trace.TraceError("{0}: {1}: {2}", Name, ex.GetType(), ex.Message);
                Trace.TraceInformation(ex.ToString());
                throw;
            }
            finally
            {
                //set current state to terminated
                Trace.TraceInformation("{0}: Thread terminated.", Name);
                State = WorkerThreadState.Terminated;
            }
        }

        /// <summary>
        /// Does the work.
        /// </summary>
        protected abstract void DoWork();

        /// <summary>
        /// Called when [starting].
        /// </summary>
        protected virtual void OnStarting() { }

        /// <summary>
        /// Called when [terminating].
        /// </summary>
        protected virtual void OnTerminating() { }

        /// <summary>
        /// Called when [disposed].
        /// </summary>
        protected virtual void OnDisposed() { }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            //terminate the thread
            this.Stop();
            _wakeEvent.Close();
            _disposed = true;

            //allow inheritors to dispose, too
            OnDisposed();
        }

        #endregion
    }
}
