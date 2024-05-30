using Godot;

namespace PixelTowns;

public class GameTime
{
    private readonly CanvasModulate canvasModulate;
    private readonly TimeSettings settings;

    private TimeData timeData;

    public GameTime(GameManager gameManager, TimeSettings settings)
    {
        this.settings = settings;
        
        canvasModulate = new CanvasModulate();
        gameManager.AddChild(canvasModulate);
    }
    
    public void Tick(float deltaTime)
    {
        timeData.NormalisedTime += settings.NormalizedTimeIncrement(deltaTime);
        canvasModulate.Color = settings.GetTimeOfDayColor(timeData.NormalisedTime);
        
        if (settings.IsForceEndOfDay(timeData.NormalisedTime))
        {
            ProgressDay();
        }
    }

    public void SetData(TimeData data)
    {
        timeData = data;
    }

    public bool IsWithinTime(float start24Hour, float end24Hour)
    {
        float time24Hour = timeData.NormalisedTime * 24f % 24;
        if (start24Hour > end24Hour)
        {
            return time24Hour >= start24Hour || time24Hour <= end24Hour;
        }
        return time24Hour >= start24Hour && time24Hour <= end24Hour;
    }

    private void ProgressDay()
    {
        timeData.Day++;
    }
}