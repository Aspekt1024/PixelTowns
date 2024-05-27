using Godot;

namespace PixelTowns.Units;

public abstract partial class Animal : Unit
{
    [Export] public AnimalData AnimalData;
}