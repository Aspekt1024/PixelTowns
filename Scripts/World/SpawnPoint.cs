using Godot;

namespace PixelTowns.World;

public partial class SpawnPoint : Node2D
{
    public void Spawn(PackedScene prefab)
    {
        Node2D obj = prefab.Instantiate<Node2D>();
        AddChild(obj);
        obj.GlobalPosition = WorldUtil.RandomCellInRadius(GlobalPosition, 5);
    }
}