using System.Collections.Generic;
using Godot;
using PixelTowns.Items;

namespace PixelTowns.InventoryManagement
{
	public partial class ItemContainer : Control, Slot.IObserver, ItemContainerData.IObserver
	{
		[Export] private GridContainer itemGrid;

		private ItemContainerData itemContainerData;
		private Slot selectedSlot;

		private readonly List<Slot> slots = new List<Slot>();
		
		internal interface IObserver
		{
			void OnSlotClicked(ItemContainer itemContainer, Slot slot);
			void OnSelectedItemDepleted(ItemContainer itemContainer);
		} 

		private readonly List<IObserver> observers = new List<IObserver>();

		internal void RegisterObserver(IObserver observer) => observers.Add(observer);
		internal void UnregisterObserver(IObserver observer) => observers.Remove(observer);

		internal void SetContainerData(ItemContainerData containerData, PackedScene slotPrefab)
		{
			if (itemContainerData != null)
			{
				itemContainerData.UnregisterObserver(this);
			}
			
			itemContainerData = containerData;
			if (itemContainerData == null) return; // TODO clear container data (required if we're going to reuse item containers)
			
			itemContainerData.RegisterObserver(this);

			for (int i = 0; i < itemContainerData.NumSlots; i++)
			{
				if (i < slots.Count)
				{
					slots[i].Clear();
				}
				else
				{
					Slot slot = slotPrefab.Instantiate<Slot>();
					slot.SetSlotIndex(i);
					itemGrid.AddChild(slot);
					slots.Add(slot);
					slot.RegisterObserver(this);
					slot.Clear();
				}
			}
			
			foreach (var slotData in itemContainerData.SlotData)
			{
				int index = slotData.SlotIndex;
				if (index >= slots.Count)
				{
					GD.PrintErr($"SlotData index ({index}) is greater than the inventory size!");
					continue;
				}
				slots[index].SetSlotData(slotData);
			}
		}

		internal void MoveSelectionLeft()
		{
			GD.PrintErr("Implement me!");
		}

		internal void MoveSelectionRight()
		{
			GD.PrintErr("Implement me!");
		}

		internal void SelectSlotAtIndex(int index)
		{
			if (index >= slots.Count)
			{
				GD.PrintErr($"Invalid slot index: {index}. Value should be between 0 and {slots.Count - 1}");
				return;
			}
			
			SelectSlot(slots[index]);
		}

		internal ItemData GetItemAtSlotIndex(int index)
		{
			if (index < 0 || index >= slots.Count)
			{
				GD.PrintErr($"invalid index given for inventory: {index}");
				return null;
			}

			return slots[index].ItemData;
		}

		internal void SelectSlot(Slot slot)
		{
			if (selectedSlot != null)
			{
				selectedSlot.Unselect();
			}

			slot.Select();
			selectedSlot = slot;
		}

		internal ItemData GetSelectedItemData()
		{
			return selectedSlot.ItemData;
		}

		/// <summary>
		/// Adds the item to the inventory if there is space.
		/// Returns the amount that did not have space to store in this inventory.
		/// </summary>
		internal int AddItem(ItemData item, int quantity = 1)
		{
			// TODO when hitting max stack size, iterate to the next slot etc
			var index = slots.FindIndex(s => s.ItemData == item);
			if (index >= 0)
			{
				slots[index].AddQuantity(quantity);
			}
			else
			{
				index = slots.FindIndex(s => s.ItemData == null);
				if (index < 0)
				{
					return quantity;
				}

				SlotData newSlotData = new SlotData(item, quantity, index);
				itemContainerData.SlotData.Add(newSlotData);
				slots[index].SetSlotData(newSlotData);
			}

			return 0; // TODO handle items over max stack size
		}

		internal void SetSlotData(Slot slot, SlotData slotData)
		{
			slotData.SlotIndex = slot.SlotIndex;
			itemContainerData.AddSlotData(slotData);
			slot.SetSlotData(slotData);
		}

		internal SlotData TakeFromSlot(Slot slot)
		{
			SlotData slotData = slot.SlotData;
			itemContainerData.RemoveSlotData(slot.SlotData);
			return slotData;
		}

		internal void UseSelectedSlot()
		{
			if (selectedSlot.ItemData is PlaceableData placeableData)
			{
				selectedSlot.RemoveQuanitity(1);
				if (selectedSlot.IsEmpty())
				{
					for (int i = observers.Count - 1; i >= 0; i--)
					{
						observers[i].OnSelectedItemDepleted(this);
					}
				}
			}
		}

		internal void UpdateSelectedSlot()
		{
			SelectSlot(selectedSlot);
		}

		internal void ToggleVisibility()
		{
			Visible = !Visible;
		}

		public void OnSlotClicked(Slot slot)
		{
			observers.ForEach(o => o.OnSlotClicked(this, slot));
		}

		public void OnSlotDataUpdated(SlotData slotData)
		{
			slots[slotData.SlotIndex].SetSlotData(slotData);
		}

		public void OnSlotDataRemoved(int slotIndex)
		{
			slots[slotIndex].Clear();
		}
	}
}