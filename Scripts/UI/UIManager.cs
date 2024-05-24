using Godot;
using PixelTowns.InventoryManagement;
using PixelTowns.ShopManagement;

namespace PixelTowns.UI;

public partial class UIManager : CanvasLayer
{
    [Export] public InventoryManager Inventory;
    [Export] public ShopUI Shop;
}