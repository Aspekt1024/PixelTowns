using System.Collections.Generic;
using Godot;
using PixelTowns.InventoryManagement;
using PixelTowns.Items;

namespace PixelTowns.ShopManagement;

public partial class ShopUI : Control, ItemContainer.IObserver, ShopItemUI.IObserver, CurrencyData.IObserver
{
    [Export] private TextureRect portrait;
    [Export] private Label moneyLabel;
    [Export] private Label greeting;
    [Export] private VBoxContainer shopItemContainer;
    [Export] private PackedScene shopItemPrefab;
    [Export] private Inventory playerInventory;

    [Export] private ShopData testShopData;

    private readonly List<ShopItemUI> shopItems = new List<ShopItemUI>();

    private PlayerData playerData;

    public override void _Ready()
    {
        Hide();
    }
    
    public void OpenShop(ShopData data, PlayerData playerData)
    {
        this.playerData = playerData;
        if (data == null)
        {
            data = testShopData;
        }

        int i = 0;
        for (; i < data.ShopItems.Count; i++)
        {
            if (i >= shopItems.Count)
            {
                ShopItemUI shopItemUi = shopItemPrefab.Instantiate<ShopItemUI>();
                shopItemContainer.AddChild(shopItemUi);
                shopItems.Add(shopItemUi);
            }
            shopItems[i].Populate(data.ShopItems[i]);
            shopItems[i].UnregisterObserver(this);
            shopItems[i].RegisterObserver(this);
        }

        // TODO remove shop items that arent needed, or at least hide them
        // TODO also need a scrollbar

        shopItemContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        
        playerInventory.SetData(playerData.InventoryData);
        playerInventory.RegisterObserver(this);

        playerData.CurrencyData.RegisterObserver(this);
        OnCurrencyModified(playerData.CurrencyData);
        
        Show();
    }

    public void CloseShop()
    {
        playerData.CurrencyData.UnregisterObserver(this);
        playerInventory.UnregisterObserver(this);
        Hide();
    }

    public void OnShopItemClicked(ShopItemUI shopItem, bool isAlternate)
    {
        CurrencyData currencyData = GameManager.SaveFile.PlayerData.CurrencyData;
        int quantity = isAlternate ? 5 : 1;
        if (currencyData.TryPurchase(shopItem.ShopItemData.ItemData, quantity))
        {
            // TODO add item to cursor
            GD.Print($"purchased {quantity} {shopItem.ShopItemData.ItemData.ResourceName}");
        }
    }
    
    public void OnSlotClicked(ItemContainer itemContainer, Slot slot)
    {
        // TODO highlight item and sell button?
    }

    public void OnSelectedItemRemoved()
    {
        // N/A for shop
    }

    public void OnCurrencyModified(CurrencyData currencyData)
    {
        moneyLabel.Text = $"{playerData.CurrencyData.Gold} G";
    }
}