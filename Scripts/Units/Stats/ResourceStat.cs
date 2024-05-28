using Godot;

namespace PixelTowns.Units;

[GlobalClass]
public partial class ResourceStat : Resource
{
    [Export] private float current = 100;
    [Export] private float max = 100;

    public float Current => current;
    public float Max => max;
    public float Ratio => current / max;

    public void Modify(float amount)
    {
        if (amount > 0f)
        {
            current = Mathf.Min(max, current + amount);
        }
        else
        {
            current = Mathf.Max(0, current + amount);
        }
    }
    
}