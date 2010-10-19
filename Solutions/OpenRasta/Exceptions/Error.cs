namespace OpenRasta.Exceptions
{
    #region Using Directives

    using System;

    using OpenRasta.Extensions;

    #endregion

    public class Error
    {
        public string Title { get; set; }

        public string Message { get; set; }

        public Exception Exception { get; set; }

        public override string ToString()
        {
            return "{0}\r\nMessage:\r\n{1}\r\n".With(this.Title, this.Message) + this.Exception != null ? "Exception:\r\n{0}".With(this.Exception) : string.Empty;
        }
    }
}