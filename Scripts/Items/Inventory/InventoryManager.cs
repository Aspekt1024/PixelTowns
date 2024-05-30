using System.Collections.Generic;
using Godot;
using PixelTowns.Items;
using PixelTowns.UI;

namespace PixelTowns.InventoryManagement;

[GlobalClass]
public partial class InventoryManager : UiBase, ItemContainer.IObserver
{
    [Export] private ItemContainer toolbelt;
    [Export] private Inventory inventory;
    [Export] private PackedScene slotPrefab;

    private InventoryData inventoryData;
    
    public interface IObserver
    {
        void OnItemSelected(ItemData item);
    }

    private readonly List<IObserver> observers = new List<IObserver>();

    public void RegisterObserver(IObserver observer) => observers.Add(observer);
    public void UnregisterObserver(IObserver observer) => observers.Remove(observer);

    private Slot heldItemSlot; // Movable slot for moving things around
    private Slot heldSlot; // Reference to the slot in an inventory that was clicked, used by heldItemSlot
    
    public override void _Ready()
    {
        heldItemSlot = slotPrefab.Instantiate<Slot>();
        AddChild(heldItemSlot);
        heldItemSlot.TopLevel = true;
        heldItemSlot.ZIndex = 100;
        heldItemSlot.Hide();
        
        inventory.Hide();
        toolbelt.Show();
        
        toolbelt.RegisterObserver(this);
        inventory.RegisterObserver(this);
    }

    public override void _Process(double delta)
    {
        if (heldItemSlot != null)
        {
            heldItemSlot.Position = heldItemSlot.GetGlobalMousePosition() - heldItemSlot.Size / 2f;
        }
    }

    public void PopulateInventory(InventoryData inventoryData)
    {
        toolbelt.SetContainerData(inventoryData.ToolbeltData, slotPrefab);
        inventory.SetData(inventoryData);
        toolbelt.SelectSlotAtIndex(0);
    }

    public void OnSlotLeftClicked(ItemContainer itemContainer, Slot slot)
    {
        if (heldSlot == null && slot.ItemData != null)
        {
            SlotData slotData = itemContainer.TakeFromSlot(slot);
            heldSlot = slot;
            heldItemSlot.SetSlotData(slotData);
            heldItemSlot.Show();
        }
        else if (heldSlot != null)
        {
            if (slot.IsEmpty())
            {
                SlotData slotData = heldItemSlot.SlotData;
                ClearHeldItemSlot();
                itemContainer.SetSlotData(slot, slotData);
                observers.ForEach(o => o.OnItemSelected(toolbelt.GetSelectedItemData())); // TODO find a better way to auto update selected slots
            }
            else
            {
                TryStackHeldItem(itemContainer, slot);
            }
        }
    }

    public void OnSlotRightClicked(ItemContainer itemContainer, Slot slot)
    {
        // NA for inventory
    }

    private void TryStackHeldItem(ItemContainer itemContainer, Slot slot)
    {
        if (slot.ItemData != heldItemSlot.ItemData) return;

        int remainingQuantity = slot.SlotData.AddQuantity(heldItemSlot.SlotData.Quantity);
        if (remainingQuantity > 0)
        {
            heldItemSlot.SlotData.SetQuantity(remainingQuantity);
        }
        else
        {
            ClearHeldItemSlot();
        }
        
        observers.ForEach(o => o.OnItemSelected(toolbelt.GetSelectedItemData())); // TODO find a better way to auto update selected slots
    }

    private void ClearHeldItemSlot()
    {
        heldSlot = null;
        heldItemSlot.Clear();
        heldItemSlot.Hide();
    }

    public void OnSelectedItemRemoved()
    {
        observers.ForEach(o => o.OnItemSelected(null));
    }

    public void SelectToolbeltSlot(int index)
    {
        toolbelt.SelectSlotAtIndex(index);
        ItemData itemData = toolbelt.GetItemAtSlotIndex(index);
        observers.ForEach(o => o.OnItemSelected(itemData));
    }

    public ItemData GetSelectedToolbeltItemData() => toolbelt.GetSelectedItemData();
    public void UseSelectedToolbeltItem() => toolbelt.UseSelectedSlot();

    public override void Open()
    {
        inventory.Show();
        toolbelt.Hide();
    }

    public override void Close()
    {
        inventory.Hide();
        toolbelt.Show();
    }
    
    /// <summary>
    /// Attempts to add the item to the inventory. If there is insufficient inventory space, returns the amount
    /// of the item that wasn't able to be added. 
    /// </summary>
    public int AddItem(ItemData itemData, int quantity) => inventory.AddItem(itemData, quantity);
}