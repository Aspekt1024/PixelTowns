using System.Collections.Generic;
using Godot;
using PixelTowns.Items;

namespace PixelTowns.InventoryManagement;

public partial class Slot : PanelContainer
{
	[Export] private TextureRect image;
	[Export] private Label label;
	[Export] private StyleBoxFlat unselectedStyleBox;
	[Export] private StyleBoxFlat selectedStyleBox;

	internal int SlotIndex { get; private set; }
	internal SlotData SlotData { get; private set; }
	
	internal interface IObserver
	{
		void OnSlotClicked(Slot slot);
	}
	
	private readonly List<IObserver> observers = new List<IObserver>();

	internal void RegisterObserver(IObserver observer) => observers.Add(observer);
	internal void UnregisterObserver(IObserver observer) => observers.Remove(observer);
	
	internal ItemData ItemData => SlotData?.ItemData;

	internal void SetSlotIndex(int index)
	{
		SlotIndex = index;
	}
	
	internal void SetSlotData(SlotData slotData)
	{
		SlotData = slotData;
		image.Texture = slotData.ItemData.GetIcon();

		if (slotData.ItemData.IsStackable())
		{
			label.Text = $"x{slotData.Quantity}";
			label.Show();	
		}
		else
		{
			label.Hide();
		}
	}

	/// <summary>
	/// Adds the amount to the quantity of this item. If this exceeds the maximum stack size,
	/// it returns the remainder above the maximum stack size. 
	/// </summary>
	internal int AddQuantity(int amount)
	{
		int amountAboveStackSize = Mathf.Max(0, SlotData.Quantity + amount - SlotData.ItemData.MaxStackSize);
		SlotData.Quantity += amount - amountAboveStackSize;
		SetSlotData(SlotData);
		return amountAboveStackSize;
	}

	internal void RemoveQuanitity(int amount)
	{
		SlotData.Quantity -= amount;
		if (SlotData.Quantity <= 0)
		{
			Clear();
		}
		else
		{
			SetSlotData(SlotData);	
		}
	}

	internal bool IsEmpty() => SlotData == null || SlotData.Quantity <= 0;

	internal bool CanRemoveQuantity(int amount)
	{
		return SlotData.Quantity >= amount;
	}

	internal void Clear()
	{
		image.Texture = null;
		label.Text = "";
		label.Hide();
		SlotData = null;
	}

	internal void Unselect()
	{
		AddThemeStyleboxOverride("panel", unselectedStyleBox);
	}

	internal void Select()
	{
		AddThemeStyleboxOverride("panel", selectedStyleBox);
	}

	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mb)
		{
			if (mb.Pressed && mb.ButtonIndex == MouseButton.Left)
			{
				observers.ForEach(o => o.OnSlotClicked(this));
			}
		}
	}
}
