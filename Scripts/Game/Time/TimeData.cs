using System.Collections.Generic;
using Godot;

namespace PixelTowns;

[GlobalClass]
public partial class TimeData : Resource
{
    [Export] private int day;
    [Export] private float normalisedTime;

    public interface IObserver
    {
        void OnDayChanged(int newDay, int daysIncremented);
        void OnTimeChanged(float normalisedTime);
    }

    private readonly List<IObserver> observers = new();

    public void RegisterObserver(IObserver observer) => observers.Add(observer);
    public void UnregisterObserver(IObserver observer) => observers.Remove(observer);
    
    public int Day
    {
        get => day;
        set
        {
            int daysIncremented = value - day;
            day = value;
            observers.ForEach(o => o.OnDayChanged(value, daysIncremented));
        }
    }

    public float NormalisedTime
    {
        get => normalisedTime;
        set
        {
            normalisedTime = value;
            observers.ForEach(o => o.OnTimeChanged(value));
        }
    }
}