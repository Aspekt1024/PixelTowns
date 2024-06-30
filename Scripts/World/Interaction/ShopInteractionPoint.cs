using Godot;
using PixelTowns.ShopManagement;

namespace PixelTowns.World;

public partial class ShopInteractionPoint : InteractionPoint
{
    [Export] private ShopData shopData;
    
    protected override void HandleInteraction()
    {
        GameManager.UI.GetUi<ShopUI>().SetData(shopData);
        GameManager.UI.OpenUi<ShopUI>();
    }
}