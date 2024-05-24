using System.Collections.Generic;
using Godot;

namespace PixelTowns;

public class DebugInputMap : GameInputMap
{
    private const string QuickSave = "quicksave";
    private const string DebugAdd = "debugAdd";
    
    private static readonly List<string> DebugMap = new()
    {
        QuickSave,
        DebugAdd,
    };

    protected override List<string> Actions => DebugMap;

    protected override void OnTick()
    {
        if (Input.IsActionJustPressed(QuickSave))
        {
            GameManager.GameData.SaveGame();
        }

        if (Input.IsActionJustPressed(DebugAdd))
        {
            // Free for use
        }
    }
}