using Godot;

namespace PixelTowns.InventoryManagement;

[GlobalClass]
public partial class InventoryData : Resource
{
    [Export] public ItemContainerData BackpackData;
    [Export] public ItemContainerData ToolbeltData;
}