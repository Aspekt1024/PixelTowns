using Godot;
using PixelTowns.InventoryManagement;
using PixelTowns.Items;

namespace PixelTowns;

public partial class Player : CharacterBody2D, InventoryManager.IObserver
{
	[Export] private float Speed = 100f;
	[Export] private InventoryData inventoryData;
	[Export] private ItemData debugAddItem;

	private Vector2 velocity = Vector2.Zero;
	
	private enum InputMode
	{
		None = 0,
		Till = 1000,
		Plant = 2000,
	}
	
	private InputMode inputMode = InputMode.None;

	public override void _Ready()
	{
		InventoryManager inventory = GameManager.UI.Inventory;

		InventoryData existingData = InventoryData.LoadData();
		if (existingData != null)
		{
			inventoryData = existingData;
		}
        
		inventory.PopulateInventory(inventoryData);
		inventory.RegisterObserver(this);
		inventory.SelectQuickbarSlot(0);
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed(InputActions.Use))
		{
			// TODO move usage to the selected item instead of having the player deal with different types and worrying about input modes.
			if (inputMode == InputMode.Till)
			{
				GameManager.WorldGrid.TillSoil();
			}
			else if (inputMode == InputMode.Plant)
			{
				bool success = GameManager.WorldGrid.TryPlaceItem(GameManager.UI.Inventory.GetSelectedQuickbarItemData() as PlaceableData);
				if (success)
				{
					GameManager.UI.Inventory.UseSelectedQuickbarItem();
				}
			}
		}
		else if (Input.IsActionJustPressed(InputActions.Interact))
		{
			GameManager.WorldGrid.GatherVegetable();
		}

		if (Input.IsActionJustPressed(InputActions.Inventory))
		{
			GameManager.UI.Inventory.ToggleInventory();
		}

		if (Input.IsActionJustPressed(InputActions.Exit))
		{
			GameManager.UI.Inventory.HideInventory();
		}

		if (Input.IsActionJustPressed(InputActions.DebugAdd))
		{
			inventoryData.Serialize();
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		velocity = Vector2.Zero;
		
		Vector2 inputVector = Vector2.Zero;
		inputVector.X = Input.GetActionStrength(InputActions.MoveRight) - Input.GetActionStrength(InputActions.MoveLeft);
		inputVector.Y = Input.GetActionStrength(InputActions.MoveDown) - Input.GetActionStrength(InputActions.MoveUp);

		velocity = inputVector.Normalized() * Speed * (float)delta;

		MoveAndCollide(velocity);
	}
	
	public void OnItemSelected(ItemData item)
	{
		if (item is ToolData tool)
		{
			switch (tool.ToolType)
			{
				case ToolType.Till:
					inputMode = InputMode.Till;
					break;
				case ToolType.Water:
					// TODO
					inputMode = InputMode.None;
					break;
				default:
					inputMode = InputMode.None;
					break;
			}
		}
		else if (item is PlaceableData)
		{
			inputMode = InputMode.Plant;
		}
		else
		{
			inputMode = InputMode.None;
		}
	}
}
