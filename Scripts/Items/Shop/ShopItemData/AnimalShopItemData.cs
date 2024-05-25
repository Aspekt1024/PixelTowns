using Godot;
using PixelTowns.Units;

namespace PixelTowns.ShopManagement;

[GlobalClass]
public partial class AnimalShopItemData : ShopItemData
{
    [Export] public AnimalData AnimalData;
    public override Resource Data => AnimalData;
    public override string ItemName => AnimalData.AnimalName.GetTranslation();
    public override Texture2D Icon => AnimalData.Icon;
    public override int GoldCost => AnimalData.GoldCost;
    
    public override void OnPurchased()
    {
        GameManager.GameData.WorldData.AddAnimal(AnimalData);
    }

}