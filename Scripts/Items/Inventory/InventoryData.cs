using Godot;

namespace PixelTowns.InventoryManagement;

[GlobalClass]
public partial class InventoryData : Resource
{
    private const string DataPath = "user://inventoryData.res";
    
    [Export] public ItemContainerData BackpackData;
    [Export] public ItemContainerData ToolbeltData;

    public void Serialize()
    {
        ResourceSaver.Save(this, DataPath);
    }

    public static InventoryData LoadData()
    {
        if (!ResourceLoader.Exists(DataPath)) return null;
        
        InventoryData data = ResourceLoader.Load<InventoryData>(DataPath);
        return data;
    }
}