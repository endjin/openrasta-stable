namespace OpenRasta.Diagnostics
{
    #region Using Directives

    using System.Diagnostics;
    using System.Linq;

    using OpenRasta.Extensions;

    #endregion

    public class DebuggerLoggingTraceListener : TraceListener
    {
        public DebuggerLoggingTraceListener() : base("DebuggerLoggingTraceListener")
        {
        }

        public override bool IsThreadSafe
        {
            get { return false; }
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            this.WriteAll(eventCache, eventType, id, data.ToString());
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            string message = string.Join(", ", data.Select(obj => obj.ToString()).ToArray());
            this.WriteAll(eventCache, eventType, id, message);
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            this.WriteAll(eventCache, eventType, id, format.With(args));
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            this.WriteAll(eventCache, eventType, id, message);
        }

        public override void Write(string message)
        {
            if (Debugger.IsLogging())
            {
                if (NeedIndent)
                {
                    WriteIndent();
                }

                Debugger.Log(0, "OpenRasta", message);
            }
        }

        public override void WriteLine(string message)
        {
            if (Debugger.IsLogging())
            {
                if (NeedIndent)
                {
                    WriteIndent();
                }

                Debugger.Log(0, "OpenRasta", message + "\r\n");
                NeedIndent = true;
            }
        }

        private void UpdateIndent()
        {
            IndentLevel = Trace.CorrelationManager.LogicalOperationStack.Count;
        }

        private void WriteAll(TraceEventCache eventCache, TraceEventType eventType, int id, string message)
        {
            this.UpdateIndent();
            this.WriteLine("{4}-[{0}] {1}({2}) {3}".With(eventCache.DateTime.ToString("u"), eventType.ToString(), id, message, eventCache.ThreadId));
        }
    }
}