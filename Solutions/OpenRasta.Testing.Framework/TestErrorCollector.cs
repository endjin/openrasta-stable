namespace OpenRasta.Testing.Framework
{
    using System.Collections.Generic;

    using OpenRasta.Diagnostics;

    public class TestErrorCollector : IErrorCollector
    {
        public IList<Error> Errors { get; private set; }

        public TestErrorCollector()
        {
            Errors = new List<Error>();
        }
        public void AddServerError(Error error)
        {
            Errors.Add(error);
        }
    }
}