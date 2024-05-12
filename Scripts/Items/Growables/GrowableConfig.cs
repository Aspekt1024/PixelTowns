using Godot;

namespace PixelTowns.Items;

[GlobalClass]
public partial class GrowableConfig : Resource
{
    [Export] private PackedScene growablePrefab;

    public Growable CreateGrowable(GrowableData growableData, Vector2I cell)
    {
        Growable growable = growablePrefab.Instantiate<Growable>();
        growable.Init(growableData, cell);
        return growable;
    }
    
}