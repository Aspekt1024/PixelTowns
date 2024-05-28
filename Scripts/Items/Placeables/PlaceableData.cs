using Godot;

namespace PixelTowns.Items;

[GlobalClass]
public partial class PlaceableData : ItemData
{
    // TODO update names of these variables so they're self explanitory
    [Export] private AtlasTexture icon; // The icon for displaying in shop
    [Export] private AtlasTexture itemTexture; // The texture for the item when placing
    [Export] public ItemData ItemData; // TODO rename this to PlacedItemData or something to indicate this is NOT the item data for this item (e.g. this = lettuce seed, and produces lettuce)
    
    public override Texture2D GetIcon()
    {
        return icon;
    }
}