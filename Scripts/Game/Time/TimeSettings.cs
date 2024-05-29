using Godot;

namespace PixelTowns;

[GlobalClass]
public partial class TimeSettings : Resource
{
    [Export] private float hoursPerSecond = 0.1f;

    // TODO make different profiles per "season"
    [Export] public float morningHour = 6.5f;
    [Export] public float dayHour = 10f;
    [Export] public float afternoonHour = 14f;
    [Export] public float nightHour = 20f;
    [Export] public float forceSleepHour = 25f;
    
    [Export] private GradientTexture1D lightingGradient;

    public float NormalizedTimeIncrement(float deltaTime)
    {
        return deltaTime * hoursPerSecond / 24f;
    }

    public bool IsForceEndOfDay(float normalisedTime)
    {
        return normalisedTime * 24 >= forceSleepHour;
    }
    
    public Color GetTimeOfDayColor(float normalisedTime)
    {
        return lightingGradient.Gradient.Sample(normalisedTime % 1f);
    }

    public static string GetTimeString(float normalisedTime)
    {
        float hours = normalisedTime * 24;
        bool isPm = false;
        if (hours >= 24)
        {
            hours -= 24;
        }
        
        if (hours >= 12)
        {
            isPm = true;
            hours -= 12;
        }

        int hour = Mathf.FloorToInt(hours);
        int minute = Mathf.FloorToInt((hours - hour) * 60f);
        return $"{hour}:{minute:00} {(isPm ? "pm" : "am")}";
    }

}