using Godot;
using PixelTowns.ShopManagement;

namespace PixelTowns.World;

public partial class InteractionPoint : Area2D
{
    [Export] private ShopData shopData;
    [Export] private CollisionObject2D collision;

    public void OnBodyEntered(Node2D other)
    {
        if (other is Player p)
        {
            GameManager.UI.GetUi<ShopUI>().SetData(shopData);
            GameManager.UI.OpenUi<ShopUI>();
        }
    }
}