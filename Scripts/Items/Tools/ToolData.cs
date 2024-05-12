using Godot;

namespace PixelTowns.Items;

public enum ToolType
{
    None = 0,
    Till = 1000,
    Water = 2000,
}

[GlobalClass]
public partial class ToolData : ItemData
{
    [Export] public ToolType ToolType;
    [Export] public AtlasTexture Texture;
    
    public override Texture2D GetIcon()
    {
        return Texture;
    }
}