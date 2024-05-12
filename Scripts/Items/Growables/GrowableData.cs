using Godot;

namespace PixelTowns.Items;

[GlobalClass]
public partial class GrowableData : ItemData
{
    [Export] public int DaysToGrow = 10;
    [Export] public SpriteFrames SpriteFrames;
    
    public override Texture2D GetIcon()
    {
        return SpriteFrames.GetFrameTexture("default", SpriteFrames.GetFrameCount("default") - 1);
    }

    public Texture2D GetFrameTexture(int index)
    {
        return SpriteFrames.GetFrameTexture("default", index);
    }
}