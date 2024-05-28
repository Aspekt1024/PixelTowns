using Godot;

namespace PixelTowns.ShopManagement;

public abstract partial class ShopItemData : Resource
{
    [Export] public int Quantity;

    public abstract string ItemName { get; }
    public abstract Texture2D Icon { get; }
    public abstract int GoldCost { get; }
    public abstract Resource Data { get; }

    public virtual void OnPurchased()
    {
        
    }
}