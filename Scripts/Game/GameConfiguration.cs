using Godot;
using PixelTowns.Items;

namespace PixelTowns;

[GlobalClass]
public partial class GameConfiguration : Resource
{
    [Export] public GrowableConfig growables;
}