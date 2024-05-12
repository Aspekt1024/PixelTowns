using Godot;
using PixelTowns;
using PixelTowns.UI;

public partial class GameManager : Node
{
	[Export] private WorldGrid worldGrid;
	[Export] private Player player;
	[Export] private GameConfiguration configuration;
	[Export] private GameResources resources;
	[Export] private UIManager ui;
	[Export] private Camera2D camera;

	private static GameManager instance;

	private GameTime time;
	
	public static WorldGrid WorldGrid => instance.worldGrid;
	public static Player Player => instance.player;
	public static GameConfiguration Config => instance.configuration;
	public static GameResources Resources => instance.resources;
	public static UIManager UI => instance.ui;
	public static Camera2D Camera => instance.camera;
	
	public GameManager()
	{
		instance = this;
		time = new GameTime();
	}

	public override void _Process(double delta)
	{
		time.Tick(delta);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyInput)
		{
			if (keyInput.Pressed)
			{
				switch (keyInput.Keycode)
				{
					case Key.Key1:
						UI.Inventory.SelectQuickbarSlot(0);
						break;
					case Key.Key2:
						UI.Inventory.SelectQuickbarSlot(1);
						break;
					case Key.Key3:
						UI.Inventory.SelectQuickbarSlot(2);
						break;
					case Key.Key4:
						UI.Inventory.SelectQuickbarSlot(3);
						break;
					case Key.Key5:
						UI.Inventory.SelectQuickbarSlot(4);
						break;
					case Key.Key6:
						UI.Inventory.SelectQuickbarSlot(5);
						break;
					case Key.Key7:
						UI.Inventory.SelectQuickbarSlot(6);
						break;
					case Key.Key8:
						UI.Inventory.SelectQuickbarSlot(7);
						break;
					case Key.Key9:
						UI.Inventory.SelectQuickbarSlot(8);
						break;
					case Key.Key0:
						UI.Inventory.SelectQuickbarSlot(9);
						break;
				}
			}
		}
	}

	public static void IncrementDay(int numDays = 1)
	{
		WorldGrid.IncrementDay(numDays);
	}
	
	public static Chicken CreateChicken()
	{
		return instance.resources.chicken.Instantiate<Chicken>();
	}

	
}

