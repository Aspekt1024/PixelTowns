using Godot;
using PixelTowns.InventoryManagement;

namespace PixelTowns;

[GlobalClass]
public partial class SaveFile : Resource
{
	private const string SavesDir = "user://saves";
	private const string DefaultFile = "testSave";
	
	[Export] public InventoryData InventoryData;
	
	public void SaveGame(string name = DefaultFile)
	{
		if (!DirAccess.DirExistsAbsolute(SavesDir))
		{
			DirAccess.MakeDirAbsolute(SavesDir);
		}

		Resource save = Duplicate(true);
		ResourceSaver.Save(save, $"{SavesDir}/{name}.res");
	}

	public static SaveFile Load(string name = DefaultFile)
	{
		string file = $"{SavesDir}/{name}.res";
		if (!ResourceLoader.Exists(file)) return null;
		
		return ResourceLoader.Load<SaveFile>(file);
	}

	public void ApplyData()
	{
		GameManager.UI.Inventory.PopulateInventory(InventoryData);
	}
}
