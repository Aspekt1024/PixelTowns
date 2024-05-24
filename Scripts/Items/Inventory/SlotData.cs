using System.Collections.Generic;
using Godot;
using PixelTowns.Items;

namespace PixelTowns.InventoryManagement;

[GlobalClass]
public partial class SlotData : Resource
{
    [Export] public ItemData ItemData;
    [Export] private int quantity;
    [Export] public int SlotIndex;
    
    public interface IObserver
    {
        void OnSlotDataUpdated(SlotData slotData);
    }

    private readonly List<IObserver> observers = new List<IObserver>();

    public void RegisterObserver(IObserver observer) => observers.Add(observer);
    public void UnregisterObserver(IObserver observer) => observers.Remove(observer);

    public int Quantity
    {
        get => quantity;
        private set
        {
            quantity = value;
            observers.ForEach(o => o.OnSlotDataUpdated(this));
        }
    }

    private SlotData() : this(null, 0, 0) { }

    internal SlotData(ItemData itemData, int quantity, int slotIndex)
    {
        ItemData = itemData;
        Quantity = quantity;
        SlotIndex = slotIndex;
    }

    /// <summary>
    /// Adds the given quantity to the slot, accounting for the max stack size.
    /// Returns the remaining quantity above the max stack size.
    /// </summary>
    internal int AddQuantity(int amount)
    {
        int newQuantity = Quantity + amount;
        int remainingQuantity = newQuantity - ItemData.MaxStackSize;
        if (remainingQuantity > 0)
        {
            Quantity = ItemData.MaxStackSize;
        }
        else
        {
            Quantity = newQuantity;
            remainingQuantity = 0;
        }

        return remainingQuantity;
    }

    internal void SetQuantity(int newQuantity)
    {
        Quantity = newQuantity;
        observers.ForEach(o => o.OnSlotDataUpdated(this));
    }
}