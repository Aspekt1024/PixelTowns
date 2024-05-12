using Godot;

namespace PixelTowns.Items;

public partial class Growable : Node2D
{
    [Export] private Sprite2D sprite;

    public GrowableData GrowableData { get; private set; }
    private Vector2I cell;
    
    private int daysInGrowth;

    public bool IsGrown { get; private set; }

    public void Init(GrowableData growableData, Vector2I cell)
    {
        this.GrowableData = growableData;
        this.cell = cell;
        
        IsGrown = false;
        daysInGrowth = 0;

        UpdateGrowthState();
    }

    public void IncrementDays(int numDays = 0)
    {
        daysInGrowth += numDays;
        UpdateGrowthState();
    }

    private void UpdateGrowthState()
    {
        int numGrowthFrames = GrowableData.SpriteFrames.GetFrameCount("default");
        float growthRatio = (float)daysInGrowth / GrowableData.DaysToGrow;
        
        int growthIndex = Mathf.Max(0, Mathf.FloorToInt(growthRatio * numGrowthFrames) - 1);
        
        if (daysInGrowth >= GrowableData.DaysToGrow)
        {
            IsGrown = true;
            growthIndex = numGrowthFrames - 1;
        }

        sprite.Texture = GrowableData.GetFrameTexture(growthIndex);
    }
}