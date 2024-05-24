using Godot;
using PixelTowns.InventoryManagement;
using PixelTowns.Items;

namespace PixelTowns;

public partial class Player : CharacterBody2D, InventoryManager.IObserver, PlayerInputMap.IObserver
{
	[Export] private float speed = 100f;

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
		GameManager.UI.GetUi<InventoryManager>().RegisterObserver(this);
		GameManager.Input.PlayerMap.RegisterObserver(this);
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 inputVector = GameManager.Input.PlayerMap.GetMovementAxis();
		velocity = inputVector * speed * (float)delta;
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

	public void OnInteractPressed()
	{
		GameManager.WorldGrid.GatherVegetable();
	}

	public void OnUsePressed()
	{
		// TODO move usage to the selected item instead of having the player deal with different types and worrying about input modes.
		if (inputMode == InputMode.Till)
		{
			GameManager.WorldGrid.TillSoil();
		}
		else if (inputMode == InputMode.Plant)
		{
			InventoryManager inventory = GameManager.UI.GetUi<InventoryManager>();
			bool success = GameManager.WorldGrid.TryPlaceItem(inventory.GetSelectedToolbeltItemData() as PlaceableData);
			if (success)
			{
				inventory.UseSelectedToolbeltItem();
			}
		}
	}
}
