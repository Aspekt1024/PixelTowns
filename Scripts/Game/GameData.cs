using Godot;
using PixelTowns.InventoryManagement;
using PixelTowns.World;

namespace PixelTowns;

/// <summary>
/// Contains all the data required to load/save games
/// </summary>
[GlobalClass]
public partial class GameData : Resource
{
	private const string SavesDir = "user://saves";
	private const string DefaultFile = "testSave";

	[Export] public PlayerData PlayerData;
	[Export] public WorldData WorldData;
	[Export] public TimeData TimeData;
	
	public void SaveGame(string name = DefaultFile)
	{
		if (!DirAccess.DirExistsAbsolute(SavesDir))
		{
			DirAccess.MakeDirAbsolute(SavesDir);
		}

		string path = $"{SavesDir}/{name}.res";
		Resource save = Duplicate(true);
		ResourceSaver.Save(save, path);
		GD.Print($"Game saved to {path}");
	}

	public static GameData Load(string name = DefaultFile)
	{
		string file = $"{SavesDir}/{name}.res";
		if (!ResourceLoader.Exists(file)) return null;
		
		return ResourceLoader.Load<GameData>(file);
	}

	public void ApplyData()
	{
		GameManager.UI.GetUi<InventoryManager>().PopulateInventory(PlayerData.InventoryData);
		GameManager.WorldGrid.SetData(WorldData);
		GameManager.Time.SetData(TimeData);
	}
}
