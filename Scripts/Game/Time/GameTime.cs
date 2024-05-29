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
        
        GD.Print(TimeSettings.GetTimeString(timeData.NormalisedTime));
        
        if (settings.IsForceEndOfDay(timeData.NormalisedTime))
        {
            ProgressDay();
        }
    }

    private void ProgressDay()
    {
        timeData.Day++;
    }

    public void SetData(TimeData data)
    {
        data ??= new TimeData(){ Day = 0, NormalisedTime = 0.271f};
        timeData = data;
    }
}