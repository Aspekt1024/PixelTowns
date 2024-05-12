using Godot;
using PixelTowns.Items;

namespace PixelTowns.InventoryManagement;

[GlobalClass]
public partial class SlotData : Resource
{
    [Export] public ItemData ItemData;
    [Export] public int Quantity;
    [Export] public int SlotIndex;

    private SlotData() : this(null, 0, 0) { }

    internal SlotData(ItemData itemData, int quantity, int slotIndex)
    {
        ItemData = itemData;
        Quantity = quantity;
        SlotIndex = slotIndex;
    }
}