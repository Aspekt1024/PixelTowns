using Godot;

namespace PixelTowns;

public static class NodeUtil
{
    public static T GetComponentInChildren<T>(this Node parent) where T : Node
    {
        foreach (var child in parent.GetChildren())
        {
            if (child is T t)
            {
                return t;
            }
        }

        return null;
    }
}