using Godot;
using Godot.Collections;
using PixelTowns;
using PixelTowns.InventoryManagement;
using PixelTowns.Items;
using PixelTowns.Units;
using PixelTowns.World;

public partial class WorldGrid : TileMap, TimeData.IObserver
{
	[Export] private Node2D cursor;
	[Export] private SpawnPoint animalSpawnPoint;

	private WorldData worldData;
	
	public enum TerrainLayer
	{
		Ground = 0,
		Top = 1,
	}

	public enum TerrainSet
	{
		Garden = 0,
	}
	
	public enum Terrain
	{
		Dirt = 0,
		Grass = 1,
	}

	public enum CustomData
	{
		IsTillable = 0,
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var pos = LocalToMap(GameManager.Player.Position);
		GameManager.Player.Position = MapToLocal(pos);
		GameManager.GameData.TimeData.RegisterObserver(this);
	}

	public override void _Process(double delta)
	{
		Vector2 cursorPos = WorldUtil.SnapToGrid(GetGlobalMousePosition());
		cursor.Position = cursorPos;
	}

	public void TillSoilAtCursor()
	{
		Vector2 cursorPos = WorldUtil.SnapToGrid(GetGlobalMousePosition());
		var cell = LocalToMap(cursorPos);
		
		var success = CreateSoil(cell);
		if (success)
		{
			worldData.AddSoil(cell);
		}
	}

	public bool TryPlaceItem(PlaceableData placeable)
	{
		if (placeable == null)
		{
			GD.PrintErr("tried to place item but placeable was null!!");
			return false;
		}
		
		Vector2 cursorPos = WorldUtil.SnapToGrid(GetGlobalMousePosition());
		var cell = LocalToMap(cursorPos);

		if (placeable.ItemData is GrowableData growableData)
		{
			return TryPlantSeed(growableData, cell);
		}
		else
		{
			// maybe a building or something
		}

		return false;
	}

	public bool TryPlantSeed(GrowableData growableData, Vector2I cell)
	{
		var growableIndex = worldData.PlantedGrowables.FindIndex(g => g.Cell == cell);
		if (growableIndex >= 0) return false;
		
		// TODO we can probably determine if this is soil another way. Maybe add an area to the sprite
		if (!worldData.SoilCells.Contains(cell)) return false;
		
		PlantedGrowableData plantedGrowableData = worldData.AddGrowableData(growableData, cell);
		AddPlantedGrowableToWorld(plantedGrowableData);
		
		return true;
	}

	public void GatherVegetable()
	{
		Vector2 cursorPos = WorldUtil.SnapToGrid(GetGlobalMousePosition());
		var cell = LocalToMap(cursorPos);
		
		var growableIndex = worldData.PlantedGrowables.FindIndex(g => g.Cell == cell);
		if (growableIndex < 0) return;

		PlantedGrowableData data = worldData.PlantedGrowables[growableIndex];
		if (data.Growable.IsGrown)
		{
			// TODO instead we should just change the inventory data and use events to drive the inventory manager
			// TODO we may need to rethink InventoryManager being a UI
			GameManager.UI.GetUi<InventoryManager>().AddItem(data.GrowableData, 1);
			data.Growable.QueueFree();
			worldData.PlantedGrowables.RemoveAt(growableIndex);
		}
		else
		{
			GD.Print("Not ready yet!");
		}
	}

	public void SetData(WorldData data)
	{
		worldData = data;
		foreach (var soilCell in data.SoilCells)
		{
			CreateSoil(soilCell);
		}

		foreach (var plantedGrowable in data.PlantedGrowables)
		{
			AddPlantedGrowableToWorld(plantedGrowable);
		}
		
		foreach (var animal in data.PurchasedAnimals)
		{
			SpawnAnimal(animal);
		}
	}

	public void SpawnAnimal(AnimalData animalData)
	{
		animalSpawnPoint.Spawn(animalData.Prefab);
	}

	public void OnDayChanged(int newDay, int daysIncremented)
	{
		IncrementDay(daysIncremented);
	}

	public void OnTimeChanged(float normalisedTime)
	{
	}

	private void IncrementDay(int numDays)
	{
		worldData.PlantedGrowables.ForEach(g => g.Growable.IncrementDays(numDays));
	}
	
	//private readonly Dictionary<Vector2I, Growable>

	private void AddPlantedGrowableToWorld(PlantedGrowableData data)
	{
		Growable growable = GameManager.Config.growables.CreateGrowable(data);
		if (growable != null)
		{
			growable.Position = MapToLocal(data.Cell) + Vector2.Up * 4;
			AddChild(growable);
		}
	}

	private bool CreateSoil(Vector2I cell)
	{
		var d = GetCellTileData((int)TerrainLayer.Ground, cell);
		bool isTillable = d.GetCustomDataByLayerId((int)CustomData.IsTillable).AsBool();
		if (isTillable)
		{
			d.TerrainSet = (int)Terrain.Dirt;
			SetCellsTerrainConnect((int)TerrainLayer.Ground, new Array<Vector2I>() {cell}, (int)TerrainSet.Garden, (int)Terrain.Dirt);
		}
		return isTillable;
	}
}
