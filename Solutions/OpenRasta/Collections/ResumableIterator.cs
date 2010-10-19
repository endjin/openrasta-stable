namespace OpenRasta.Collections
{
    #region Using Directives

    using System;
    using System.Collections;
    using System.Collections.Generic;

    #endregion

    public class ResumableIterator<T, TKey> : IEnumerator<T>, IEnumerable<T>
    {
        private readonly Func<TKey, TKey, bool> equalityProvider;
        private readonly Func<T, TKey> keyProvider;
        private readonly IEnumerator<T> sourceEnumerator;
        private bool isAhead;
        private bool isSuspended;
        private TKey suspendAfter;

        public ResumableIterator(IEnumerator<T> source, Func<T, TKey> keyProvider, Func<TKey, TKey, bool> equalityProvider)
        {
            this.sourceEnumerator = source;
            this.keyProvider = keyProvider;
            this.equalityProvider = equalityProvider;
        }

        public T Current
        {
            get { return this.sourceEnumerator.Current; }
        }

        object IEnumerator.Current
        {
            get { return this.sourceEnumerator.Current; }
        }

        public bool ResumeFrom(TKey key)
        {
            if (ReferenceEquals(key, null))
            {
                throw new ArgumentNullException("key");
            }

            if (this.CurrentKeyIs(key))
            {
                return true;
            }

            while (this.sourceEnumerator.MoveNext())
            {
                if (this.CurrentKeyIs(key))
                {
                    this.isAhead = true;
                 
                    return true;
                }
            }

            return false;
        }

        public void SuspendAfter(TKey key)
        {
            if (ReferenceEquals(key, null))
            {
                throw new ArgumentNullException("key");
            }

            this.suspendAfter = key;
        }

        void IDisposable.Dispose()
        {
            this.sourceEnumerator.Dispose();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this;
        }

        bool IEnumerator.MoveNext()
        {
            if (this.isSuspended)
            {
                this.isSuspended = false;
                return false;
            }

            if (this.isAhead)
            {
                this.isAhead = false;
                return true;
            }

            if (this.sourceEnumerator.MoveNext())
            {
                var newKey = this.keyProvider(this.sourceEnumerator.Current);

                if (!ReferenceEquals(this.suspendAfter, null) && this.equalityProvider(newKey, this.suspendAfter))
                {
                    this.isSuspended = true;
                }

                return true;
            }

            return false;
        }

        void IEnumerator.Reset()
        {
            // ignore the reset as we may continue iterating later on.
        }

        private bool CurrentKeyIs(TKey key)
        {
            if (ReferenceEquals(this.Current, null))
            {
                return false;
            }

            return this.equalityProvider(this.keyProvider(this.Current), key);
        }
    }
}