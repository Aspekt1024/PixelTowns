using Godot;
using Godot.Collections;
using PixelTowns.Units;

namespace PixelTowns.World;

[GlobalClass]
public partial class WorldData : Resource
{
    [Export] public Array<AnimalData> PurchasedAnimals;

    public void AddAnimal(AnimalData animalData)
    {
        PurchasedAnimals.Add(animalData);
        GameManager.WorldGrid.SpawnAnimal(animalData);
    }
}