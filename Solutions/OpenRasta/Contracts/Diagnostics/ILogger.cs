// ReSharper disable UnusedTypeParameter
namespace OpenRasta.Contracts.Diagnostics
{
    using System;

    public interface ILogger
    {
        IDisposable Operation(object source, string name);

        void WriteDebug(string message, params object[] format);

        void WriteWarning(string message, params object[] format);

        void WriteError(string message, params object[] format);

        void WriteInfo(string message, params object[] format);

        void WriteException(Exception e);
    }

    public interface ILogger<TLogSource> : ILogger where TLogSource : ILogSource
    {
    }

    public interface ILogSource
    {
    }
// ReSharper restore UnusedTypeParameter
}