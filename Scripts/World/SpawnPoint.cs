using Godot;

namespace PixelTowns.World;

public partial class SpawnPoint : Node2D
{
    [Export] private PackedScene chickenPrefab;
    
    public void SpawnChickens(int numChickens)
    {
        for (int i = 0; i < numChickens; i++)
        {
            Chicken chicken = chickenPrefab.Instantiate<Chicken>();
            AddChild(chicken);
            chicken.GlobalPosition = WorldUtil.RandomCellInRadius(GlobalPosition, 5);
        }
    }
}