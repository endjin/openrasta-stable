namespace OpenRasta.Collections
{
    using System;

    public class CollectionChangedEventArgs<T> : EventArgs
    {
        public CollectionChangedEventArgs(T item)
        {
            this.Item = item;
        }

        public T Item { get; private set; }
    }
}