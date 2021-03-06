﻿// Copyright (c) Microsoft Corporation.  All rights reserved.
// This source code is made available under the terms of the Microsoft Public License (MS-PL)

namespace OpenRasta.Reflection
{
    using System.Collections.Generic;

    public class ScopedDictionary<TKey, TValue>
    {
        private ScopedDictionary<TKey, TValue> previous;
        private Dictionary<TKey, TValue> map;

        public ScopedDictionary() : this(null)
        {
        }

        public ScopedDictionary(ScopedDictionary<TKey, TValue> previous)
        {
            this.previous = previous;
            this.map = new Dictionary<TKey, TValue>();
        }

        public ScopedDictionary(ScopedDictionary<TKey, TValue> previous, IEnumerable<KeyValuePair<TKey, TValue>> pairs) : this(previous)
        {
            foreach (var p in pairs)
            {
                this.map.Add(p.Key, p.Value);
            }
        }

        public void Add(TKey key, TValue value)
        {
            this.map.Add(key, value);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            for (ScopedDictionary<TKey, TValue> scope = this; scope != null; scope = scope.previous)
            {
                if (scope.map.TryGetValue(key, out value))
                {
                    return true;
                }
            }

            value = default(TValue);
            
            return false;
        }

        public bool ContainsKey(TKey key)
        {
            for (ScopedDictionary<TKey, TValue> scope = this; scope != null; scope = scope.previous)
            {
                if (scope.map.ContainsKey(key))
                {
                    return true;
                }
            }

            return false;
        }
    }
}