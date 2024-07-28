using Godot;
using PixelTowns.World;

namespace PixelTowns.Items;

[GlobalClass]
public partial class GrowableConfig : Resource
{
    [Export] private PackedScene growablePrefab;

    public Growable CreateGrowable(PlantedGrowableData plantedGrowableData)
    {
        if (plantedGrowableData.Growable != null)
        {
            GD.PrintErr($"{nameof(PlantedGrowableData)} already has an attached growable but {nameof(CreateGrowable)} was called on it again.");
            return null;
        }
        
        Growable growable = growablePrefab.Instantiate<Growable>();
        plantedGrowableData.Init(growable);
        
        return growable;
    }
    
}