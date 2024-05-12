using Godot;

namespace PixelTowns;

[GlobalClass]
public partial class GameResources : Resource
{
    [Export] public PackedScene growable;
    [Export] public PackedScene chicken;
}