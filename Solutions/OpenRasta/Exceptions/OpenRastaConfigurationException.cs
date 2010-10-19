namespace OpenRasta.Exceptions
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text;

    #endregion

    [Serializable]
    public class OpenRastaConfigurationException : Exception
    {
        public OpenRastaConfigurationException()
        {
        }

        public OpenRastaConfigurationException(string message) : base(message)
        {
        }

        public OpenRastaConfigurationException(string message, Exception inner) : base(message, inner)
        {
        }

        public OpenRastaConfigurationException(IList<OpenRastaConfigurationException> exceptions)
            : base("Several configuration errors were reported. See below.\n" + GetInnerExceptionMessages(exceptions))
        {
            this.InnerExceptions = exceptions;
        }

        protected OpenRastaConfigurationException(SerializationInfo info,  StreamingContext context) : base(info, context)
        {
        }

        public IList<OpenRastaConfigurationException> InnerExceptions { get; private set; }

        private static string GetInnerExceptionMessages(IList<OpenRastaConfigurationException> exceptions)
        {
            var finalString = new StringBuilder();
            
            foreach (var exception in exceptions)
            {
                finalString.AppendLine(exception.ToString());
                finalString.AppendLine("-------------------------");
            }

            return finalString.ToString();
        }
    }
}