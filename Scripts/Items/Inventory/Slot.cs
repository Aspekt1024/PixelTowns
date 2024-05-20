using System.Collections.Generic;
using Godot;
using PixelTowns.Items;

namespace PixelTowns.InventoryManagement;

public partial class Slot : PanelContainer, SlotData.IObserver
{
	[Export] private TextureRect image;
	[Export] private Label label;
	[Export] private StyleBoxFlat unselectedStyleBox;
	[Export] private StyleBoxFlat selectedStyleBox;
	[Export] private StyleBoxFlat transitionalStyleBox;

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
		SlotData.RegisterObserver(this);

		PopulateUi();
	}

	/// <summary>
	/// Sets the mode to transitional, e.g. for moving items around.
	/// This turns off the border and allows the cursor to click through.
	/// </summary>
	internal void SetTransitionalMode()
	{
		AddThemeStyleboxOverride("panel", transitionalStyleBox);
		MouseFilter = MouseFilterEnum.Ignore;
	}

	internal int AddQuantity(int quantity) => SlotData.AddQuantity(quantity);

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

		if (SlotData != null)
		{
			SlotData.UnregisterObserver(this);
			SlotData = null;
		}
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

	public void OnSlotDataUpdated(SlotData slotData)
	{
		PopulateUi();
	}

	private void PopulateUi()
	{
		image.Texture = SlotData.ItemData.GetIcon();

		if (SlotData.ItemData.IsStackable())
		{
			label.Text = $"x{SlotData.Quantity}";
			label.Show();	
		}
		else
		{
			label.Hide();
		}
	}
}
