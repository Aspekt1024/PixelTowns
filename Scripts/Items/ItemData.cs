using Godot;
using PixelTowns.Translation;

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
    [Export] public TranslationKey ItemName = new TranslationKey();
    
    /// <summary>
    /// Maximum amount this item can be stored in an inventory.
    /// Zero or 1 for not stackable.
    /// </summary>
    [Export] public int MaxStackSize;

    [Export] public int GoldCost;

    [Export] public ItemCategory Category;

    public virtual bool CanPlaceInInventory => false;
    
    public abstract Texture2D GetIcon();

    /// <summary>
    /// Gets the name of the item from the translation server
    /// </summary>
    public string GetName() => ItemName.GetTranslation();

    public bool IsStackable() => MaxStackSize > 1;

    public virtual void OnPurchased() { }
}