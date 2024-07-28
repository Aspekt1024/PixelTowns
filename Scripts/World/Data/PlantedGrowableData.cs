using System.Collections.Generic;
using Godot;
using PixelTowns.Items;

namespace PixelTowns.World;

[GlobalClass]
public partial class PlantedGrowableData : Resource
{
    [Export] public Vector2I Cell;
    [Export] public GrowableData GrowableData;
    
    [Export] private int daysInGrowth;

    public Growable Growable { get; private set; }
    
    public interface IObserver
    {
        void OnDaysInGrowthChanged(int daysInGrowth);
    }

    private readonly List<IObserver> observers = new List<IObserver>();

    public void RegisterObserver(IObserver observer) => observers.Add(observer);
    public void UnregisterObserver(IObserver observer) => observers.Remove(observer);

    public void Init(Growable growable)
    {
        Growable = growable;
        growable.Init(this);
    }

    public int DaysInGrowth
    {
        get => daysInGrowth;
        set
        {
            daysInGrowth = value;
            observers.ForEach(o => o.OnDaysInGrowthChanged(daysInGrowth));
        }
    }
}