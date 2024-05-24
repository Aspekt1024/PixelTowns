using System.Collections.Generic;
using Godot;

namespace PixelTowns;

public abstract class GameInputMap
{
    protected abstract List<string> Actions { get; }

    protected bool IsEnabled { get; private set; }

    public void Enable()
    {
        IsEnabled = true;
    }
    
    public void Disable()
    {
        IsEnabled = false;
    }

    public void Tick()
    {
        if (!IsEnabled) return;
        OnTick();
    }

    public void PhysicsTick()
    {
        if (!IsEnabled) return;
        OnPhysicsTick();
    }

    public void InputEvent(InputEvent @event)
    {
        if (!IsEnabled) return;
        OnInputEvent(@event);
    }

    protected virtual void OnTick() { }
    protected virtual void OnPhysicsTick() { }
    protected virtual void OnInputEvent(InputEvent @event) { }
}