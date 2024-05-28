using System;
using System.Collections.Generic;

namespace PixelTowns;

public static class ListUtil
{
    public static void ForEachReverse<T>(this List<T> list, Action<T> action)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            action.Invoke(list[i]);
        }
    }
}