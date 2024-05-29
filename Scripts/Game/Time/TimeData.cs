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
        void OnDayChanged(int day);
        void OnTimeChanged(float normalisedTime);
    }

    private readonly List<IObserver> observers = new();

    public void RegisterObserver(IObserver observer)
    {
        observers.Add(observer);
        
        observer.OnDayChanged(Day);
        observer.OnTimeChanged(NormalisedTime);
    }
    
    public void UnregisterObserver(IObserver observer) => observers.Remove(observer);
    
    public int Day
    {
        get => day;
        set
        {
            day = value;
            observers.ForEach(o => o.OnDayChanged(value));
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