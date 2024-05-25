using Godot;

namespace PixelTowns;

public class Random
{
    private static Random instance;
    
    private RandomNumberGenerator rng;

    public Random(ulong seed)
    {
        instance = this;
        rng = new RandomNumberGenerator();

        if (seed > 0)
        {
            rng.Seed = seed;   
        }
    }
    
    public static int Range(int start, int endExclusive)
    {
        return instance.rng.RandiRange(start, endExclusive - 1);
    }

    public static float Range(float start, float endInclusive)
    {
        return instance.rng.RandfRange(start, endInclusive);
    }

    public static Vector2 InUnitCircle(float size)
    {
        return new Vector2(Range(-1f, 1f), Range(-1f, 1f)).Normalized() * size;
    }
}