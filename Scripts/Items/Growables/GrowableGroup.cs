using Godot;
using Godot.Collections;

namespace PixelTowns.Items;


[GlobalClass]
public partial class GrowableGroup : Resource
{
    [Export] public Texture2D Texture;
    [Export] public int HFrames;
    [Export] public int VFrames;
    [Export] public Array<GrowableData> Growables;
}