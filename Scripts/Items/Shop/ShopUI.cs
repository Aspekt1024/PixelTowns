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
    [Export] private Slot transactionSlot;

    [Export] private ShopData testShopData;

    private readonly List<ShopItemUI> shopItems = new List<ShopItemUI>();

    private PlayerData playerData;

    public override void _Ready()
    {
        transactionSlot.Hide();
        transactionSlot.SetTransitionalMode();
        Hide();
    }

    public override void _Process(double delta)
    {
        if (transactionSlot.Visible)
        {
            transactionSlot.Position = transactionSlot.GetGlobalMousePosition() - transactionSlot.Size / 2f;
        }
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
            GD.Print($"purchased {quantity} {shopItem.ShopItemData.ItemData.ResourceName}");
            transactionSlot.Show();
            if (transactionSlot.IsEmpty())
            {
                transactionSlot.SetSlotData(new SlotData(shopItem.ShopItemData.ItemData, quantity, 0));   
            }
            else if (transactionSlot.ItemData == shopItem.ShopItemData.ItemData)
            {
                transactionSlot.SlotData.AddQuantity(quantity);
            }
        }
    }
    
    public void OnSlotClicked(ItemContainer itemContainer, Slot slot)
    {
        // TODO highlight item and sell button?
        int remainingQuantity = itemContainer.AddItem(transactionSlot.ItemData, transactionSlot.SlotData.Quantity);
        if (remainingQuantity > 0)
        {
            transactionSlot.SlotData.SetQuantity(remainingQuantity);
        }
        else
        {
            transactionSlot.Clear();
            transactionSlot.Hide();
        }
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