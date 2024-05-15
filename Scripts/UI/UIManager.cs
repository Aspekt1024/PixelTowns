using Godot;
using PixelTowns.InventoryManagement;
using PixelTowns.ShopManagement;

namespace PixelTowns.UI;

public partial class UIManager : Control
{
    [Export] public InventoryManager Inventory;
    [Export] public ShopUI Shop;
}