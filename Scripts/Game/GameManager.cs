using Godot;
using PixelTowns;
using PixelTowns.InventoryManagement;
using PixelTowns.UI;

public partial class GameManager : Node
{
	[Export] private WorldGrid worldGrid;
	[Export] private Player player;
	[Export] private GameConfiguration configuration;
	[Export] private GameResources resources;
	[Export] private UIManager ui;
	[Export] private Camera2D camera;
	
	// TODO create starting equipment data system
	[Export] private InventoryData inventoryData;

	private static GameManager instance;

	private readonly GameTime time;
	private SaveFile saveFile;
	
	public static WorldGrid WorldGrid => instance.worldGrid;
	public static Player Player => instance.player;
	public static GameConfiguration Config => instance.configuration;
	public static GameResources Resources => instance.resources;
	public static UIManager UI => instance.ui;
	public static Camera2D Camera => instance.camera;
	public static SaveFile SaveFile => instance.saveFile;
	
	public GameManager()
	{
		instance = this;
		time = new GameTime();
	}

	public override void _Ready()
	{
		saveFile = SaveFile.Load() ?? new SaveFile()
		{
			InventoryData = inventoryData,
		};
		saveFile.ApplyData();
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
						UI.Inventory.SelectToolbeltSlot(0);
						break;
					case Key.Key2:
						UI.Inventory.SelectToolbeltSlot(1);
						break;
					case Key.Key3:
						UI.Inventory.SelectToolbeltSlot(2);
						break;
					case Key.Key4:
						UI.Inventory.SelectToolbeltSlot(3);
						break;
					case Key.Key5:
						UI.Inventory.SelectToolbeltSlot(4);
						break;
					case Key.Key6:
						UI.Inventory.SelectToolbeltSlot(5);
						break;
					case Key.Key7:
						UI.Inventory.SelectToolbeltSlot(6);
						break;
					case Key.Key8:
						UI.Inventory.SelectToolbeltSlot(7);
						break;
					case Key.Key9:
						UI.Inventory.SelectToolbeltSlot(8);
						break;
					case Key.Key0:
						UI.Inventory.SelectToolbeltSlot(9);
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

