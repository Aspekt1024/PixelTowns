using System.Collections.Generic;
using Godot;

namespace PixelTowns;

public class InputManager
{
    private const float DeadZone = 0.1f;

    private PlayerInputMap playerMap = new();
    private UiInputMap uiMap = new();
    private PlayerInputMap playerMap = new();

    private readonly List<IGameInputMap> inputMaps = new()
    {
        
    };
    
    public void EnableMap(List<string> map)
    {
        foreach (var action in map)
        {
            InputMap.AddAction(action, DeadZone);
        }
    }
    
    public void DisableMap(List<string> map)
    {
        foreach (var action in map)
        {
            InputMap.EraseAction(action);
        }
    }
}