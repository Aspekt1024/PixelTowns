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

    public void Run()
    {
        AiOverseer.Log($"{GetType().Name} started");
        Begin();
    }
    
    public abstract void Tick(double deltaTime);
    public abstract float GetUtility();
    
    protected abstract void Begin();

    protected void CompleteAction()
    {
        AiOverseer.Log($"{GetType().Name} complete");
        observers.ForEachReverse(o => o.OnActionComplete(this));
    }
}
