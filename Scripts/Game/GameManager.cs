using Godot;
using Godot.Collections;
using PixelTowns;
using PixelTowns.UI;

public partial class GameManager : Node
{
	[Export] private WorldGrid worldGrid;
	[Export] private Player player;
	[Export] private GameConfiguration configuration;
	[Export] private PackedScene uiPrefab;
	[Export] private Camera2D camera;
	
	[Export] private PlayerData startingPlayerData;

	private static GameManager instance;

	private readonly GameTime time;
	private SaveFile gameData;
	private UIManager ui;
	
	private readonly GameState gameState = new();
	private readonly InputManager input = new();
	
	public static WorldGrid WorldGrid => instance.worldGrid;
	public static Player Player => instance.player;
	public static GameConfiguration Config => instance.configuration;
	public static UIManager UI => instance.ui;
	public static Camera2D Camera => instance.camera;
	public static SaveFile GameData => instance.gameData;
	public static InputManager Input => instance.input;
	public static GameState State => instance.gameState;
	
	public GameManager()
	{
		instance = this;
		time = new GameTime();
	}

	public override void _Ready()
	{
		ui = uiPrefab.Instantiate<UIManager>();
		AddChild(ui);
        
		gameData = SaveFile.Load() ?? new SaveFile()
		{
			PlayerData = startingPlayerData,
		};
		gameData.ApplyData();

		gameState.StartGame();
	}

	public override void _Process(double delta)
	{
		time.Tick(delta);
		input.Tick();
	}

	public override void _PhysicsProcess(double delta)
	{
		input.PhysicsTick();
	}

	public static void IncrementDay(int numDays = 1)
	{
		WorldGrid.IncrementDay(numDays);
	}

	public override Array<Dictionary> _GetPropertyList()
	{
		var props = base._GetPropertyList();
		var l = TranslationServer.GetLocale();
		var to = TranslationServer.GetTranslationObject(l);
		GD.Print(to.GetMessageCount());
		return props;
	}

	public override void _Input(InputEvent @event)
	{
		Input.OnInputEvent(@event);
	}
}

