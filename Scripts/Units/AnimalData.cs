using Godot;
using PixelTowns.Translation;

namespace PixelTowns.Units;

[GlobalClass]
public partial class AnimalData : Resource
{
    [Export] public TranslationKey AnimalName;
    [Export] public Texture2D Icon;
    [Export] public int GoldCost;
    [Export] public PackedScene Prefab;
}