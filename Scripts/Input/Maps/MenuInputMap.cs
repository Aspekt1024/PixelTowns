using System.Collections.Generic;
using Godot;
using PixelTowns.InventoryManagement;

namespace PixelTowns;

public class MenuInputMap : GameInputMap
{
    private const string Inventory = "inventory";
    private const string Exit = "exit";
    
    private static readonly List<string> MenuMap = new()
    {
        Exit,
        Inventory,
    };

    protected override List<string> Actions => MenuMap;

    protected override void OnTick()
    {
        if (Input.IsActionJustPressed(Inventory))
        {
            GameManager.UI.OpenUi<InventoryManager>();
        }

        if (Input.IsActionJustPressed(Exit))
        {
            GameManager.UI.OnCancelPressed();
        }
    }
}