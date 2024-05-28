using System.Collections.Generic;

namespace PixelTowns.Units;

public abstract class AiAction
{
    public interface IObserver
    {
        void OnActionComplete(AiAction action);
    }

    private readonly List<IObserver> observers = new();

    public void RegisterObserver(IObserver o) => observers.Add(o);
    public void UnregisterObserver(IObserver o) => observers.Remove(o);

    public Unit Unit { get; private set; }

    public AiAction(Unit unit)
    {
        Unit = unit;
    }

    public void Run()
    {
        AiOverseer.LogInfo($"{Unit.Name} - {GetType().Name} started");
        Begin();
    }
    
    public abstract void Tick(float deltaTime);
    public abstract float GetUtility();
    
    protected abstract void Begin();

    protected void CompleteAction()
    {
        AiOverseer.LogInfo($"{Unit.Name} - {GetType().Name} complete");
        observers.ForEachReverse(o => o.OnActionComplete(this));
    }
}
