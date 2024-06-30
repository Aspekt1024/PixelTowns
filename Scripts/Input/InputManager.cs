using System.Collections.Generic;
using Godot;

namespace PixelTowns;

public class InputManager
{
    public readonly PlayerInputMap PlayerMap = new();
    public readonly UiInputMap UiMap = new();
    public readonly MenuInputMap MenuMap = new();
    public readonly DebugInputMap DebugMap = new();

    private readonly List<GameInputMap> inputMaps;
        
    public InputManager()
    {
        inputMaps = new List<GameInputMap>
        {
            PlayerMap,
            UiMap,
            MenuMap,
            DebugMap,
        };
    }

    public void EnableDefaultMaps()
    {
        PlayerMap.Enable();
        MenuMap.Enable();
        DebugMap.Enable();
    }
    
    public void Tick()
    {
        inputMaps.ForEach(m => m.Tick());
    }

    public void PhysicsTick()
    {
        inputMaps.ForEach(m => m.PhysicsTick());
    }
    
    public void OnInputEvent(InputEvent @event)
    {
        inputMaps.ForEach(m => m.InputEvent(@event));
    }

    public static bool IsMouseInteract(InputEvent @event)
    {
        return @event is InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.Right };
    }
    
}