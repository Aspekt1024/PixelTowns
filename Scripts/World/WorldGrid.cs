using System.Collections.Generic;
using Godot;
using Godot.Collections;
using PixelTowns.InventoryManagement;
using PixelTowns.Items;
using PixelTowns.World;

public partial class WorldGrid : TileMap
{
	[Export] private Node2D cursor;
	[Export] private SpawnPoint chickenSpawnPoint;
	
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
	
	private readonly List<Vector2I> soilCells = new List<Vector2I>();
	private readonly List<PlantedGrowable> growables = new List<PlantedGrowable>();

	private class PlantedGrowable
	{
		public Vector2I Cell;
		public Growable Growable;
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var pos = LocalToMap(GameManager.Player.Position);
		GameManager.Player.Position = MapToLocal(pos);
	}

	public override void _Process(double delta)
	{
		Vector2 cursorPos = WorldUtil.SnapToGrid(GetGlobalMousePosition());
		cursor.Position = cursorPos;
	}

	public void IncrementDay(int numDays)
	{
		growables.ForEach(g => g.Growable.IncrementDays(numDays));
	}

	public void TillSoil()
	{
		Vector2 cursorPos = WorldUtil.SnapToGrid(GetGlobalMousePosition());
		var cell = LocalToMap(cursorPos);
		
		var d = GetCellTileData((int)TerrainLayer.Ground, cell);
		bool isTillable = d.GetCustomDataByLayerId((int)CustomData.IsTillable).AsBool();
		if (isTillable)
		{
			d.TerrainSet = (int)Terrain.Dirt;
			SetCellsTerrainConnect((int)TerrainLayer.Ground, new Array<Vector2I>() {cell}, (int)TerrainSet.Garden, (int)Terrain.Dirt);
			
			soilCells.Add(cell);
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
		var growableIndex = growables.FindIndex(g => g.Cell == cell);
		if (growableIndex >= 0) return false;
		
		// TODO we can probably determine if this is soil another way. Maybe add an area to the sprite
		if (!soilCells.Contains(cell)) return false;
		
		Growable growable = GameManager.Config.growables.CreateGrowable(growableData, cell);
		growable.Position = MapToLocal(cell) + Vector2.Up * 4;
		
		growables.Add(new PlantedGrowable(){ Cell = cell, Growable = growable});
		AddChild(growable);
		return true;
	}

	public void GatherVegetable()
	{
		Vector2 cursorPos = WorldUtil.SnapToGrid(GetGlobalMousePosition());
		var cell = LocalToMap(cursorPos);
		
		var growableIndex = growables.FindIndex(g => g.Cell == cell);
		if (growableIndex < 0) return;

		Growable growable = growables[growableIndex].Growable;
		if (growable.IsGrown)
		{
			// TODO instead we should just change the inventory data and use events to drive the inventory manager
			// TODO we may need to rethink InventoryManager being a UI
			GameManager.UI.GetUi<InventoryManager>().AddItem(growable.GrowableData, 1);
			growables[growableIndex].Growable.QueueFree();
			growables.RemoveAt(growableIndex);
		}
		else
		{
			GD.Print("Not ready yet!");
		}
	}

	public void SetData(WorldData data)
	{
		chickenSpawnPoint.SpawnChickens(data.numChickens);
	}
}
