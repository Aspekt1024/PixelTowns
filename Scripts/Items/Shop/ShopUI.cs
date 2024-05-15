using System.Collections.Generic;
using Godot;

namespace PixelTowns.ShopManagement;

public partial class ShopUI : Control
{
    [Export] private ColorRect portrait;
    [Export] private Label moneyLabel;
    [Export] private Label greeting;
    [Export] private VBoxContainer shopItemContainer;
    [Export] private PackedScene shopItemPrefab;

    private readonly List<ShopItemUI> shopItems = new List<ShopItemUI>();

    public override void _Ready()
    {
        Hide();
    }

    public void Populate(ShopItemList itemList)
    {
        for (int i = 0; i < itemList.ShopItems.Count; i++)
        {
            if (i >= shopItems.Count)
            {
                ShopItemUI shopItemUi = shopItemPrefab.Instantiate<ShopItemUI>();
                shopItemContainer.AddChild(shopItemUi);
                shopItems.Add(shopItemUi);
            }
            shopItems[i].Populate(itemList.ShopItems[i]);
        }
    }
}