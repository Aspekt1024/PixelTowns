using Godot;
using PixelTowns.World;

namespace PixelTowns.Items;

public partial class Growable : Node2D, PlantedGrowableData.IObserver
{
    [Export] private Sprite2D sprite;

    public PlantedGrowableData Data { get; private set; }
    
    public bool IsGrown { get; private set; }

    public void Init(PlantedGrowableData data)
    {
        Data = data;
        IsGrown = false;
        OnDaysInGrowthChanged(data.DaysInGrowth);
        
        data.RegisterObserver(this);
    }

    public void IncrementDays(int numDays = 0)
    {
        Data.DaysInGrowth += numDays;
    }

    public void OnDaysInGrowthChanged(int daysInGrowth)
    {
        int numGrowthFrames = Data.GrowableData.SpriteFrames.GetFrameCount("default");
        float growthRatio = (float)daysInGrowth / Data.GrowableData.DaysToGrow;
        
        int growthIndex = Mathf.Max(0, Mathf.FloorToInt(growthRatio * numGrowthFrames) - 1);
        
        if (daysInGrowth >= Data.GrowableData.DaysToGrow)
        {
            IsGrown = true;
            growthIndex = numGrowthFrames - 1;
        }

        sprite.Texture = Data.GrowableData.GetFrameTexture(growthIndex);
    }
}