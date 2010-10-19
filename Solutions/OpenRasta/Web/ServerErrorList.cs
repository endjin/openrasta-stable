namespace OpenRasta.Web
{
    using System;
    using System.Collections.ObjectModel;

    using OpenRasta.Contracts.Diagnostics;
    using OpenRasta.Diagnostics;
    using OpenRasta.Exceptions;

    public class ServerErrorList : Collection<Error>
    {
        private ILogger log;

        public ServerErrorList()
        {
            this.Log = new NullLogger();
        }

        public ILogger Log
        {
            get { return this.log; }
            set { this.log = value ?? new NullLogger(); }
        }

        protected override void InsertItem(int index, Error item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            if (item.Exception != null)
            {
                this.Log.WriteException(item.Exception);
            }
            else
            {
                this.Log.WriteError(item.Message);
            }

            base.InsertItem(index, item);
        }
    }
}