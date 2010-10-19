namespace OpenRasta.Testing.Framework
{
    using System.Collections.Generic;

    using OpenRasta.Contracts;
    using OpenRasta.Diagnostics;
    using OpenRasta.Exceptions;

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