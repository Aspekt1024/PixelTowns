using System.Collections.Generic;
using Godot;

namespace PixelTowns.ShopManagement;

public partial class ShopItemUI : Control
{
    [Export] private TextureRect icon;
    [Export] private Label nameLabel;
    [Export] private Label costLabel;

    public ShopItemData ShopItemData { get; private set; }

    internal interface IObserver
    {
        void OnShopItemClicked(ShopItemUI shopItemUi, bool isAlternate);
    }

    private readonly List<IObserver> observers = new List<IObserver>();

    internal void RegisterObserver(IObserver o) => observers.Add(o);
    internal void UnregisterObserver(IObserver o) => observers.Remove(o);
    
    internal void Populate(ShopItemData shopItemData)
    {
        ShopItemData = shopItemData;
        icon.Texture = shopItemData.ItemData.GetIcon();
        nameLabel.Text = shopItemData.ItemData.ResourceName;
        costLabel.Text = $"{shopItemData.ItemData.GoldCost} G";

        SizeFlagsHorizontal = SizeFlags.ExpandFill;
    }
    
    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mb)
        {
            if (mb.Pressed && mb.ButtonIndex == MouseButton.Left)
            {
                observers.ForEach(o => o.OnShopItemClicked(this, false));
            }
            else if (mb.Pressed && mb.ButtonIndex == MouseButton.Right)
            {
                observers.ForEach(o => o.OnShopItemClicked(this, true));
            }
        }
    }
}