namespace OpenRasta.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Provides an iterator that can notify on elements being selected or discarded.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObservableIterator<T> : IEnumerable<T>
    {
        private readonly IEqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;
        private readonly Func<IEnumerable<T>, IEnumerable<T>> filter;
        private readonly Action<T> onDiscarded;
        private readonly Action<T> onSelected;
        private readonly IEnumerable<T> target;
        private T currentInnerItem;
        private T currentOuterItem;

        public ObservableIterator(IEnumerable<T> target, Func<IEnumerable<T>, IEnumerable<T>> filter, Action<T> onSelected, Action<T> onDiscarded)
        {
            this.target = target;
            this.filter = filter;
            this.onSelected = onSelected;
            this.onDiscarded = onDiscarded;
        }

        protected bool EnumeratedElementsMatch
        {
            get { return this.equalityComparer.Equals(this.currentInnerItem, this.currentOuterItem); }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            var filter = this.filter(this.WrapperEnumerator(this.target));

            foreach (var outerItem in filter)
            {
                this.currentOuterItem = outerItem;
                
                if (this.onSelected != null)
                {
                    this.onSelected(outerItem);
                }

                yield return outerItem;
            }
        }

        private void NotifyItemDiscardedIfNecessary()
        {
            if (!this.EnumeratedElementsMatch)
            {
                if (this.onDiscarded != null)
                {
                    this.onDiscarded(this.currentInnerItem);
                }
            }
        }

        private IEnumerable<T> WrapperEnumerator(IEnumerable<T> enumerable)
        {
            bool isFirst = true;
            foreach (var item in enumerable)
            {
                if (!isFirst)
                {
                    this.NotifyItemDiscardedIfNecessary();
                }
                else
                {
                    isFirst = false;
                }

                this.currentInnerItem = item;
                
                yield return item;
            }

            this.NotifyItemDiscardedIfNecessary();
        }
    }
}