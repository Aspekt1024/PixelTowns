using Godot;
using PixelTowns.Items;

namespace PixelTowns.InventoryManagement;

/// <summary>
/// The player's inventory, consisting of all item containers.
/// For individual "inventories", see <see cref="ItemContainer"/>, which contains all the items for a specific
/// container.
/// </summary>
public partial class Inventory : Control
{
    [Export] private ItemContainer toolbeltContainer;
    [Export] private ItemContainer backpackContainer;

    internal void RegisterObserver(ItemContainer.IObserver observer)
    {
        toolbeltContainer.RegisterObserver(observer);
        backpackContainer.RegisterObserver(observer);
    }

    public void SetData(InventoryData inventoryData, PackedScene slotPrefab)
    {
        toolbeltContainer.SetContainerData(inventoryData.ToolbeltData, slotPrefab);
        backpackContainer.SetContainerData(inventoryData.BackpackData, slotPrefab);
    }

    public int AddBackpackItem(ItemData itemData, int quanitity)
    {
        return backpackContainer.AddItem(itemData, quanitity);
    }

    public void ToggleVisibility()
    {
        Visible = !Visible;
    }
}