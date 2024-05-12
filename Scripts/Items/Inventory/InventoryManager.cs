using System.Collections.Generic;
using Godot;
using PixelTowns.Items;

namespace PixelTowns.InventoryManagement;

[GlobalClass]
public partial class InventoryManager : Node, ItemContainer.IObserver
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

    private Slot heldItemSlot;
    private Slot heldSlot;
    
    public override void _Ready()
    {
        heldItemSlot = slotPrefab.Instantiate<Slot>();
        AddChild(heldItemSlot);
        heldItemSlot.TopLevel = true;
        heldItemSlot.ZIndex = 100;
        heldItemSlot.Hide();
        
        inventory.Hide();
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
        inventory.SetData(inventoryData, slotPrefab);
    }

    public void OnSlotClicked(ItemContainer itemContainer, Slot slot)
    {
        if (heldSlot == null && slot.ItemData != null)
        {
            SlotData slotData = itemContainer.TakeFromSlot(slot);
            heldSlot = slot;
            heldItemSlot.SetSlotData(slotData);
            heldItemSlot.Show();
        }
        else if (heldSlot != null && slot.IsEmpty()) // TODO handle when slot is of the same type and is stackable
        {
            SlotData slotData = heldItemSlot.SlotData;
            heldItemSlot.Clear();
            
            itemContainer.SetSlotData(slot, slotData);
            heldSlot = null;
            heldItemSlot.Hide();
            observers.ForEach(o => o.OnItemSelected(toolbelt.GetSelectedItemData()));
        }
    }

    public void OnSelectedItemDepleted(ItemContainer itemContainer)
    {
        observers.ForEach(o => o.OnItemSelected(null));
    }

    public void SelectQuickbarSlot(int index)
    {
        toolbelt.SelectSlotAtIndex(index);
        ItemData itemData = toolbelt.GetItemAtSlotIndex(index);
        observers.ForEach(o => o.OnItemSelected(itemData));
    }

    public ItemData GetSelectedQuickbarItemData() => toolbelt.GetSelectedItemData();
    public void UseSelectedQuickbarItem() => toolbelt.UseSelectedSlot();
    public void ToggleInventory()
    {
        inventory.ToggleVisibility();
        toolbelt.ToggleVisibility();
    }

    public void HideInventory()
    {
        inventory.Hide();
        toolbelt.Show();
    }

    public void AddItem(ItemData itemData, int quantity)
    {
        int remainingQuantity = toolbelt.AddItem(itemData, quantity);
        if (remainingQuantity > 0)
        {
            remainingQuantity = inventory.AddBackpackItem(itemData, remainingQuantity);
            if (remainingQuantity > 0)
            {
                GD.PrintErr("Handle case when inventory is full!!!");
            }
        }
    }
}