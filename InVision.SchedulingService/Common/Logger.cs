using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Emdat.Diagnostics
{
    /// <summary>
    /// Logger
    /// </summary>
    public static class Logger
    {
        private static TraceSource _traceSource;

        /// <summary>
        /// Gets or sets the trace source.
        /// </summary>
        /// <value>The trace source.</value>
        public static TraceSource TraceSource
        {
            get
            {
                return _traceSource;
            }
            set
            {
                if (value != null)
                {
                    List<TraceListener> eventLogTraceListeners = new List<TraceListener>();
                    foreach (TraceListener t in value.Listeners)
                    {
                        if (t is EventLogTraceListener)
                        {
                            eventLogTraceListeners.Add(t);
                        }
                    }
                    foreach (var t in eventLogTraceListeners)
                    {
                        value.Listeners.Remove(t);
                    }
                    value.Listeners.AddRange(eventLogTraceListeners.ToArray());
                }
                _traceSource = value;
            }
        }

        /// <summary>
        /// Traces the event.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="id">The id.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public static void TraceEvent(TraceEventType eventType, int id, string format, params object[] args)
        {
            if (TraceSource != null)
            {
                try
                {
                    Logger.TraceSource.TraceEvent(eventType, 0, format, args);
                    Logger.TraceSource.Flush();
                }
                catch (System.ComponentModel.Win32Exception ex)
                {
                    //TFS Bug #5697: the EventLog file is full - just swallow the exception!
                    if (ex.NativeErrorCode != 1502)
                    {
                        throw;
                    }
                }
            }
        }
    }
}
