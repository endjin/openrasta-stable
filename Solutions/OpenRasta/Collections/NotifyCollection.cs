namespace OpenRasta.Collections
{
    using System;
    using System.Collections.ObjectModel;

    public class NotifyCollection<T> : Collection<T>
    {
        public readonly EventHandler<CollectionChangedEventArgs<T>> ItemAdded = (src, e) => { };
        public readonly EventHandler<CollectionChangedEventArgs<T>> ItemRemoved = (src, e) => { };

        protected override void ClearItems()
        {
            this.ForEach(item => this.ItemRemoved(this, new CollectionChangedEventArgs<T>(item)));
            base.ClearItems();
        }

        protected override void InsertItem(int index, T item)
        {
            this.OnItemInsert(index, item);
            this.ItemAdded(this, new CollectionChangedEventArgs<T>(item));
        }

        protected virtual void OnItemAdd(int index, T item)
        {
            base.SetItem(index, item);
        }

        protected void OnItemInsert(int index, T item)
        {
            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            this.OnItemRemoved(index);
            this.ItemRemoved(this, new CollectionChangedEventArgs<T>(this[index]));
        }

        protected override void SetItem(int index, T item)
        {
            if (!ReferenceEquals(this[index], null))
            {
                this.ItemRemoved(this, new CollectionChangedEventArgs<T>(this[index]));
            }

            this.OnItemAdd(index, item);
            this.ItemAdded(this, new CollectionChangedEventArgs<T>(item));
        }

        private void OnItemRemoved(int index)
        {
            base.RemoveItem(index);
        }
    }
}