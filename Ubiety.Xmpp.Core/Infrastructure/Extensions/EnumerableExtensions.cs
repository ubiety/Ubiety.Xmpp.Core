using System;
using System.Collections.Generic;

namespace Ubiety.Xmpp.Core.Infrastructure.Extensions
{
    /// <summary>
    ///     Enumerable extensions
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        ///     Applies an action on each item in the enumerable
        /// </summary>
        /// <typeparam name="T">Type of the item</typeparam>
        /// <param name="enumerable">Enumerable to iterate</param>
        /// <param name="action">Action to perform</param>
        public static void Apply<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action.Invoke(item);
            }
        }
    }
}
