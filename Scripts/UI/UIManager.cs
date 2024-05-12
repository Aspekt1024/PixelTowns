using Godot;
using PixelTowns.InventoryManagement;

namespace PixelTowns.UI;

public partial class UIManager : CanvasLayer
{
    [Export] public InventoryManager Inventory;
}