using Godot;
using PixelTowns.InventoryManagement;
using PixelTowns.Items;

namespace PixelTowns;

[GlobalClass]
public partial class PlayerData : Resource
{
    [Export] public InventoryData InventoryData;
    [Export] public CurrencyData CurrencyData;
}