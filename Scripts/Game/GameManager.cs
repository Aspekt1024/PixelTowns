using Godot;
using Godot.Collections;
using PixelTowns;
using PixelTowns.UI;

public partial class GameManager : Node
{
	[Export] private WorldGrid worldGrid;
	[Export] private Player player;
	[Export] private GameConfiguration configuration;
	[Export] private GameResources resources;
	[Export] private PackedScene uiPrefab;
	[Export] private Camera2D camera;
	
	[Export] private PlayerData startingPlayerData;

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

	private UIManager ui;
	
	public GameManager()
	{
		instance = this;
		time = new GameTime();
	}

	public override void _Ready()
	{
		ui = uiPrefab.Instantiate<UIManager>();
		AddChild(ui);
        
		saveFile = SaveFile.Load() ?? new SaveFile()
		{
			PlayerData = startingPlayerData,
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

	public override Array<Dictionary> _GetPropertyList()
	{
		var props = base._GetPropertyList();
		var l = TranslationServer.GetLocale();
		var to = TranslationServer.GetTranslationObject(l);
		GD.Print(to.GetMessageCount());
		return props;
	}
}

