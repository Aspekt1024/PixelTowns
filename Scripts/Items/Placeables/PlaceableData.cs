using Godot;

namespace PixelTowns.Items;

[GlobalClass]
public partial class PlaceableData : ItemData
{
    [Export] private AtlasTexture icon;
    [Export] private AtlasTexture itemTexture;
    [Export] public ItemData ItemData;
    
    public override Texture2D GetIcon()
    {
        return icon;
    }
}