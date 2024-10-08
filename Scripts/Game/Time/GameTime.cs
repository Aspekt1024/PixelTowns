﻿using Godot;

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
        float time24Hour = GetTimeFromNormalized(timeData.NormalisedTime);
        if (start24Hour > end24Hour)
        {
            return time24Hour >= start24Hour || time24Hour <= end24Hour;
        }
        return time24Hour >= start24Hour && time24Hour <= end24Hour;
    }

    public void ProgressDay()
    {
        GameManager.UI.Transition.FadeToBlack(() =>
        {
            timeData.Day++;
            timeData.NormalisedTime = GetNormalizedTime(settings.morningHour);
            GameManager.UI.Transition.FadeToScene(null);
        });
    }

    private float GetNormalizedTime(float time) => time / settings.hoursPerDay;
    private float GetTimeFromNormalized(float normalisedTime) => normalisedTime * settings.hoursPerDay % settings.hoursPerDay;
}