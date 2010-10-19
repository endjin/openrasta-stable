namespace OpenRasta.Data
{
    using System;
    using System.Collections.Generic;

    public class PagedData<T>
    {
        public int CurrentPage { get; set; }

        public IList<T> Items { get; set; }

        public IList<PagedData<T>> OtherPages { get; set; }

        public int PageSize { get; set; }

        public Uri PageUri { get; set; }

        public int ResultCount { get; set; }
    }
}