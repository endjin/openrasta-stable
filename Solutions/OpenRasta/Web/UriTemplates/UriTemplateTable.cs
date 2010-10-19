namespace OpenRasta.Web.UriTemplates
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel; 

    #endregion

    public class UriTemplateTable
    {
        private readonly List<KeyValuePair<UriTemplate, object>> keyValuePairs;
        private ReadOnlyCollection<KeyValuePair<UriTemplate, object>> keyValuePairsReadOnly;

        public UriTemplateTable() : this(null, null)
        {
        }

        public UriTemplateTable(IEnumerable<KeyValuePair<UriTemplate, object>> keyValuePairs) : this(null, keyValuePairs)
        {
        }

        public UriTemplateTable(Uri baseAddress) : this(baseAddress, null)
        {
        }

        public UriTemplateTable(Uri baseAddress, IEnumerable<KeyValuePair<UriTemplate, object>> keyValuePairs)
        {
            this.BaseAddress = baseAddress;
            this.keyValuePairs = keyValuePairs != null ? new List<KeyValuePair<UriTemplate, object>>(keyValuePairs) : new List<KeyValuePair<UriTemplate, object>>();
        }

        public Uri BaseAddress { get; set; }

        public bool IsReadOnly { get; private set; }

        public IList<KeyValuePair<UriTemplate, object>> KeyValuePairs
        {
            get
            {
                return this.IsReadOnly
                           ? this.keyValuePairsReadOnly
                           : (IList<KeyValuePair<UriTemplate, object>>)this.keyValuePairs;
            }
        }

        public void MakeReadOnly(bool allowDuplicateEquivalentUriTemplates)
        {
            if (this.BaseAddress == null)
            {
                throw new InvalidOperationException("You need to set a BaseAddress before calling MakeReadOnly");
            }

            if (!allowDuplicateEquivalentUriTemplates)
            {
                this.EnsureAllTemplatesAreDifferent();
            }

            this.IsReadOnly = true;
            this.keyValuePairsReadOnly = this.keyValuePairs.AsReadOnly();
        }

        public Collection<UriTemplateMatch> Match(Uri uri)
        {
            // TODO: Rewrite to leverage a tree shape for the matching process
            int lastMaxLiteralSegmentCount = 0;
            var matches = new Collection<UriTemplateMatch>();
            
            foreach (var template in this.KeyValuePairs)
            {
                UriTemplateMatch potentialMatch = template.Key.Match(this.BaseAddress, uri);

                if (potentialMatch != null)
                {
                    // this calculates and keep only what matches the maximum possible amount of literal segments
                    int currentMaxLiteralSegmentCount = potentialMatch.RelativePathSegments.Count - potentialMatch.WildcardPathSegments.Count;

                    for (int i = 0; i < potentialMatch.BoundVariables.Count; i++)
                    {
                        if (potentialMatch.QueryParameters == null || potentialMatch.QueryParameters[potentialMatch.BoundVariables.GetKey(i)] == null)
                        {
                            currentMaxLiteralSegmentCount -= 1;
                        }
                    }

                    potentialMatch.Data = template.Value;

                    if (currentMaxLiteralSegmentCount > lastMaxLiteralSegmentCount)
                    {
                        lastMaxLiteralSegmentCount = currentMaxLiteralSegmentCount;
                        matches.Clear();
                    }
                    else if (currentMaxLiteralSegmentCount < lastMaxLiteralSegmentCount)
                    {
                        continue;
                    }

                    matches.Add(potentialMatch);
                }
            }

            return matches;
        }

        public UriTemplateMatch MatchSingle(Uri uri)
        {
            UriTemplateMatch singleMatch = null;

            foreach (var segmentKey in this.KeyValuePairs)
            {
                UriTemplateMatch potentialMatch = segmentKey.Key.Match(this.BaseAddress, uri);

                if (potentialMatch != null && singleMatch != null)
                {
                    throw new UriTemplateMatchException("Several matching templates were found.");
                }

                if (potentialMatch != null)
                {
                    singleMatch = potentialMatch;
                    singleMatch.Data = segmentKey.Value;
                }
            }

            return singleMatch;
        }

        private void EnsureAllTemplatesAreDifferent()
        {
            // highly unoptimized, but good enough for now. It's an O(n!) in all cases
            // if you wnat to implement a sort algorythm on this, be my guest. It's only called
            // once per application lifecycle so not sure there's much value.
            for (int i = 0; i < this.keyValuePairs.Count; i++)
            {
                KeyValuePair<UriTemplate, object> rootKey = this.keyValuePairs[i];
                
                for (int j = i + 1; j < this.keyValuePairs.Count; j++)
                {
                    if (rootKey.Key.IsEquivalentTo(this.keyValuePairs[j].Key))
                    {
                        throw new InvalidOperationException("Two equivalent templates were found.");
                    }
                }
            }
        }
    }
}