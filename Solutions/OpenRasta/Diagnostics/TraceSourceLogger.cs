namespace OpenRasta.Diagnostics
{
    #region Using Directives

    using System;
    using System.Diagnostics;

    using OpenRasta.Contracts.Diagnostics;
    using OpenRasta.Extensions;

    #endregion

    public class TraceSourceLogger : ILogger
    {
        readonly TraceSource source;

        public TraceSourceLogger() : this(new TraceSource("openrasta"))
        {
        }

        public TraceSourceLogger(TraceSource source)
        {
            this.source = source;
            this.source.Listeners.Remove("Default");

            var listener = new DebuggerLoggingTraceListener
            {
                Name = "OpenRasta", 
                TraceOutputOptions =
                    TraceOptions.DateTime | TraceOptions.ThreadId |
                    TraceOptions.LogicalOperationStack
            };

            this.source.Listeners.Add(listener);

            this.source.Switch = new SourceSwitch("OpenRasta", "All");
        }

        public IDisposable Operation(object source, string name)
        {
            this.source.TraceData(TraceEventType.Start, 1, "Entering {0}: {1}".With(source.GetType().Name, name));
            Trace.CorrelationManager.StartLogicalOperation(source.GetType().Name);

            return new OperationCookie { Initiator = source, Source = this.source };
        }

        public void WriteDebug(string message, params object[] format)
        {
            this.source.TraceData(TraceEventType.Verbose, 0, message.With(format));
        }

        public void WriteError(string message, params object[] format)
        {
            this.source.TraceData(TraceEventType.Error, 0, message.With(format));
        }

        public void WriteException(Exception e)
        {
            if (e == null)
            {
                return;
            }

            this.WriteError("An error of type {0} has been thrown", e.GetType());
            
            foreach (string line in e.ToString().Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                this.WriteError(line);
            }
        }

        public void WriteInfo(string message, params object[] format)
        {
            this.source.TraceData(TraceEventType.Information, 0, message.With(format));
        }

        public void WriteWarning(string message, params object[] format)
        {
            this.source.TraceData(TraceEventType.Warning, 0, message.With(format));
        }

        private class OperationCookie : IDisposable
        {
            public object Initiator { get; set; }

            public TraceSource Source { get; set; }

            public void Dispose()
            {
                Trace.CorrelationManager.StopLogicalOperation();
                this.Source.TraceData(TraceEventType.Stop, 1, "Exiting {0}".With(this.Initiator.GetType().Name));
            }
        }
    }
}