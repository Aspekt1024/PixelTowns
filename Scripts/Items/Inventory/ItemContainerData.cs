using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace PixelTowns.InventoryManagement;

[GlobalClass]
public partial class ItemContainerData : Resource
{
    [Export] public int NumSlots;
    [Export] public Array<SlotData> SlotData;

    internal interface IObserver
    {
        void OnSlotDataUpdated(SlotData slotData);
        void OnSlotDataRemoved(int slotIndex);
    }

    private readonly List<IObserver> observers = new List<IObserver>();
    
    internal void RegisterObserver(IObserver o) => observers.Add(o);
    internal void UnregisterObserver(IObserver o) => observers.Remove(o);
    
    /// <summary>
    /// Adds the slot data to the item container. If the container's slot number already contains
    /// an item in this space, it will be overridden.
    /// </summary>
    internal void AddSlotData(SlotData slotData)
    {
        bool isSlotIndexPresent = false;
        for (int i = 0; i < SlotData.Count; i++)
        {
            if (SlotData[i].SlotIndex == slotData.SlotIndex)
            {
                SlotData[i] = slotData;
                isSlotIndexPresent = true;
                break;
            }
        }

        if (!isSlotIndexPresent)
        {
            SlotData.Add(slotData);
        }
        
        observers.ForEach(o => o.OnSlotDataUpdated(slotData));
    }
    
    internal int AddQuantityToSlot(SlotData data, int amount)
    {
        return data.AddQuantity(amount);
    }

    internal void RemoveQuantityFromSlot(SlotData data, int amount)
    {
        int newQuantity = data.Quantity - amount;
        if (newQuantity > 0)
        {
            data.SetQuantity(newQuantity);
        }
        else
        {
            RemoveSlotData(data);
        }
    }

    /// <summary>
    /// Removes the slot data from the item container.
    /// </summary>
    internal void RemoveSlotData(SlotData slotData)
    {
        int slotIndex = slotData.SlotIndex;
        for (int i = 0; i < SlotData.Count; i++)
        {
            if (SlotData[i].SlotIndex == slotIndex)
            {
                SlotData.RemoveAt(i);
                observers.ForEach(o => o.OnSlotDataRemoved(slotIndex));
                return;
            }
        }
        
        GD.PrintErr($"slot index {slotData.SlotIndex} does not exist in data. Item container data was probably modified outside of {nameof(AddSlotData)} and {nameof(RemoveSlotData)}.");
    }
}