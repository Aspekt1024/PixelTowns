using System;
using Godot;
using Godot.Collections;

namespace PixelTowns;

public static class ArrayUtil
{
    public static int FindIndex<[MustBeVariant] T>(this Array<T> array, Predicate<T> predicate)
    {
        for (int i = 0; i < array.Count; i++)
        {
            if (predicate.Invoke(array[i]))
            {
                return i;
            }
        }
        
        return -1;
    }

    public static void ForEach<[MustBeVariant] T>(this Array<T> array, Action<T> action)
    {
        foreach (var item in array)
        {
            action.Invoke(item);
        }
    }
}