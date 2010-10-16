namespace OpenRasta.Hosting
{
    using System.Collections;
    using System.Runtime.Remoting.Messaging;

    public class AmbientContext
    {
        private readonly Hashtable items = new Hashtable();

        public static AmbientContext Current
        {
            get { return CallContext.HostContext as AmbientContext; }
        }

        public object this[string key]
        {
            get { return this.items[key]; }
            set { this.items[key] = value; }
        }
    }
}