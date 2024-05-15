using System.Collections.Generic;
using Godot;

namespace PixelTowns.ShopManagement;

public partial class ShopItemUI : Control
{
    [Export] private TextureRect icon;
    [Export] private Label nameLabel;
    [Export] private Label costLabel;

    private ShopItemData shopItemData;

    internal interface IObserver
    {
        void OnShopItemClicked(ShopItemUI shopItemUi);
    }

    private readonly List<IObserver> observers = new List<IObserver>();

    internal void RegisterObserver(IObserver o) => observers.Add(o);
    internal void UnregisterObserver(IObserver o) => observers.Remove(o);
    
    internal void Populate(ShopItemData shopItemData)
    {
        this.shopItemData = shopItemData;
        icon.Texture = shopItemData.ItemData.GetIcon();
    }
}