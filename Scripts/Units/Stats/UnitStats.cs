using System.Collections.Generic;
using Godot;

namespace PixelTowns.Units;

[GlobalClass]
public partial class UnitStats : Resource
{
    [Export] public ResourceStat Energy;

    private readonly List<IStatEffect> effects = new();

    public void Tick(float deltaTime)
    {
        effects.ForEachReverse(e => e.Tick(deltaTime));
    }

    public void AddEffect(IStatEffect effect)
    {
        effects.Add(effect);
    }

    public void RemoveEffect(IStatEffect effect)
    {
        effects.Remove(effect);
    }
}