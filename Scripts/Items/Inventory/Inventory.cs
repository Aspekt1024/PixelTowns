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

    public int AddItem(ItemData itemData, int quantity)
    {
        int remainingQuantity = toolbeltContainer.AddItem(itemData, quantity);
        if (remainingQuantity > 0)
        {
            remainingQuantity = backpackContainer.AddItem(itemData, remainingQuantity);
        }
        return remainingQuantity;
    }

    public void ToggleVisibility()
    {
        Visible = !Visible;
    }
}