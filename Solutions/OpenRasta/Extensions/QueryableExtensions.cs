namespace OpenRasta.Extensions
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OpenRasta.Data;

    #endregion

    public static class QueryableExtensions
    {
        public static PagedData<T> SelectPagedData<T>(this IQueryable<T> source, int requestedPage, int pageSize)
        {
            return SelectPagedData(source, requestedPage, pageSize, null);
        }

        public static PagedData<T> SelectPagedData<T>(this IQueryable<T> source, int requestedPage, int pageSize, Func<PagedData<T>, Uri> pageUriCreator)
        {
            if (requestedPage < 1)
            {
                throw new ArgumentOutOfRangeException("requestedPage", "The requested page cannot be less than 1");
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException("pageSize", "The page size cannot be less than 1");
            }

            int totalItemCount = source.Count();
            int totalPageCount = (totalItemCount / pageSize) + (totalItemCount % pageSize > 0 ? 1 : 0);

            // assign null value
            pageUriCreator = pageUriCreator ?? ((t) => null);

            if (requestedPage != 1 && requestedPage > totalPageCount)
            {
                throw new ArgumentOutOfRangeException(
                    "requestedPage", string.Format("There is no page {0}", requestedPage));
            }

            var currentPage = new PagedData<T> { CurrentPage = requestedPage, PageSize = pageSize };
            currentPage.PageUri = pageUriCreator(currentPage);
            
            var availablePages = new List<PagedData<T>>();
            
            for (int i = 1; i <= totalPageCount; i++)
            {
                if (i == requestedPage)
                {
                    continue;
                }
                
                var newPage = new PagedData<T> { CurrentPage = i, PageSize = pageSize };
                newPage.PageUri = pageUriCreator(newPage);
                availablePages.Add(newPage);
            }

            currentPage.OtherPages = availablePages;

            var start = (requestedPage == 1) ? source : source.Skip((requestedPage - 1) * pageSize);
            currentPage.Items = start.Take(pageSize).ToList();
            currentPage.ResultCount = totalItemCount;

            return currentPage;
        }
    }
}