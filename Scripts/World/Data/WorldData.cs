using Godot;
using Godot.Collections;
using PixelTowns.Items;
using PixelTowns.Units;

namespace PixelTowns.World;

[GlobalClass]
public partial class WorldData : Resource
{
    [Export] public Array<Vector2I> SoilCells;
    [Export] public Array<PlantedGrowableData> PlantedGrowables;
    [Export] public Array<AnimalData> PurchasedAnimals;

    public void AddSoil(Vector2I cell)
    {
        SoilCells.Add(cell);
    }

    public PlantedGrowableData AddGrowableData(GrowableData data, Vector2I cell)
    {
        PlantedGrowableData plantedGrowableData = new PlantedGrowableData
        {
            Cell = cell,
            GrowableData = data,
            DaysInGrowth = 0,
        };
        
        PlantedGrowables.Add(plantedGrowableData);

        return plantedGrowableData;
    }

    public void AddAnimal(AnimalData animalData)
    {
        PurchasedAnimals.Add(animalData);
        GameManager.WorldGrid.SpawnAnimal(animalData);
    }
}