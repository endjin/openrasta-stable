﻿namespace OpenRasta.Extensions
{
    using System;
    using System.Collections.Generic;

    public static class FuncExtensions
    {
        public static Func<T, T> Chain<T>(this IEnumerable<Func<T, T>> functions)
        {
            Func<T, T> root = null;
            
            foreach (var func in functions)
            {
                if (root == null)
                {
                    root = func;
                }
                else
                {
                    var current = root;
                    var next = func;
                    root = x => next(current(x));
                }
            }

            return root;
        }
    }
}