using Godot;
using PixelTowns.Items;

namespace PixelTowns.ShopManagement;

[GlobalClass]
public partial class ShopItemData : Resource
{
    [Export] public ItemData ItemData;
    [Export] public int Quantity;
}