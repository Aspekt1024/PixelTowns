using Godot;

namespace PixelTowns.Items;

public enum ItemCategory
{
    None = 0,
    Tool = 1000,
    Seed = 2000,
    Produce = 3000,
}

public abstract partial class ItemData : Resource
{
    /// <summary>
    /// Maximum amount this item can be stored in an inventory.
    /// Zero or 1 for not stackable.
    /// </summary>
    [Export] public int MaxStackSize;

    [Export] public int GoldCost;

    [Export] public ItemCategory Category;
    
    public abstract Texture2D GetIcon();

    public bool IsStackable() => MaxStackSize > 1;
}