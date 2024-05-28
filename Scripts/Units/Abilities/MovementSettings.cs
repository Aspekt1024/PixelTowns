using Godot;

namespace PixelTowns.Units;

[GlobalClass]
public partial class MovementSettings : Resource
{
    [Export] public float moveSpeed = 20f;
}