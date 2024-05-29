using Godot;
using Godot.Collections;
using PixelTowns;
using PixelTowns.UI;
using PixelTowns.Units;

public partial class GameManager : Node
{
	[Export] private WorldGrid worldGrid;
	[Export] private Player player;
	[Export] private GameConfiguration configuration;
	[Export] private PackedScene uiPrefab;
	[Export] private Camera2D camera;
	
	[Export] private GameData startingGameData;
	[Export] private TimeSettings timeSettings;

	private static GameManager instance;

	private GameTime time;
	private GameData gameData;
	private UIManager ui;
	
	private readonly GameState gameState = new();
	private readonly InputManager input = new();
	private readonly Random random = new(0);
	private readonly AiOverseer aiOverseer = new();
	
	public static WorldGrid WorldGrid => instance.worldGrid;
	public static Player Player => instance.player;
	public static GameConfiguration Config => instance.configuration;
	public static GameTime Time => instance.time;
	public static UIManager UI => instance.ui;
	public static Camera2D Camera => instance.camera;
	public static GameData GameData => instance.gameData;
	public static InputManager Input => instance.input;
	public static GameState State => instance.gameState;

	public static Random Random => instance.random;
	public static AiOverseer Ai => instance.aiOverseer;
	
	public GameManager()
	{
		instance = this;
		Ai.SetLogMode(AiOverseer.LogMode.Normal);
	}

	public override void _Ready()
	{
		time = new GameTime(this, timeSettings);
		
		ui = uiPrefab.Instantiate<UIManager>();
		AddChild(ui);

		gameData = GameData.Load() ?? startingGameData;
		gameData.ApplyData();

		gameState.StartGame();
	}

	public override void _Process(double delta)
	{
		time.Tick((float)delta);
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

