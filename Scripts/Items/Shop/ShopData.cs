using Godot;
using Godot.Collections;

namespace PixelTowns.ShopManagement;

[GlobalClass]
public partial class ShopData : Resource
{
    [Export] public Texture2D Portrait;
    [Export] public string Greeting;
    [Export] public Array<ShopItemData> ShopItems;
}