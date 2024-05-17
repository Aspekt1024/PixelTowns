using Godot;
using Godot.Collections;

namespace PixelTowns.ShopManagement;

[GlobalClass]
public partial class ShopItemList : Resource
{
    [Export] public Array<ShopItemData> ShopItems;
}