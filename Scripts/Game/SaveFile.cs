using Godot;
using PixelTowns.InventoryManagement;

namespace PixelTowns;

[GlobalClass]
public partial class SaveFile : Resource
{
	private const string SavesDir = "user://saves";
	private const string DefaultFile = "testSave";

	[Export] public PlayerData PlayerData;
	
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

	public static SaveFile Load(string name = DefaultFile)
	{
		string file = $"{SavesDir}/{name}.res";
		if (!ResourceLoader.Exists(file)) return null;
		
		return ResourceLoader.Load<SaveFile>(file);
	}

	public void ApplyData()
	{
		GameManager.UI.GetUi<InventoryManager>().PopulateInventory(PlayerData.InventoryData);
	}
}
