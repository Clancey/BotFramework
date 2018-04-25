using System;
using System.Collections.Generic;

namespace BotFramework
{
    public static class LinqExtensions
    {
        public static void ForEach<T>(this List<T> list, Action<T> action)
        {
            if (list == null)
                return;
            foreach (var item in list)
                action?.Invoke(item);
        }
    }
}