using Godot;

namespace PixelTowns.Items;

public abstract partial class ItemData : Resource
{
    /// <summary>
    /// Maximum amount this item can be stored in an inventory.
    /// Zero or 1 for not stackable.
    /// </summary>
    [Export] public int MaxStackSize;
    
    public abstract Texture2D GetIcon();

    public bool IsStackable() => MaxStackSize > 1;
}