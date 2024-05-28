using Godot;
using PixelTowns.Items;

namespace PixelTowns.ShopManagement;

[GlobalClass]
public partial class StandardShopItemData : ShopItemData
{
    [Export] public ItemData ItemData;

    public override Resource Data => ItemData;
    public override string ItemName => ItemData.ItemName.GetTranslation();
    public override Texture2D Icon => ItemData.GetIcon();
    public override int GoldCost => ItemData.GoldCost;
}