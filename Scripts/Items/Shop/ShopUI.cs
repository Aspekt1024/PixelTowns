using System.Collections.Generic;
using Godot;
using PixelTowns.InventoryManagement;
using PixelTowns.Items;
using PixelTowns.UI;

namespace PixelTowns.ShopManagement;

public partial class ShopUI : UiBase, ItemContainer.IObserver, ShopItemUI.IObserver, CurrencyData.IObserver
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

    public void SetData(ShopData data)
    {
        playerData = GameManager.GameData.PlayerData;
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
        playerInventory.UnregisterObserver(this);
        playerInventory.RegisterObserver(this);

        playerData.CurrencyData.UnregisterObserver(this);
        playerData.CurrencyData.RegisterObserver(this);
        OnCurrencyModified(playerData.CurrencyData);
    }

    public override void Close()
    {
        playerData.CurrencyData.UnregisterObserver(this);
        playerInventory.UnregisterObserver(this);
        base.Close();
    }

    public void OnShopItemClicked(ShopItemUI shopItem, bool isAlternate)
    {
        CurrencyData currencyData = GameManager.GameData.PlayerData.CurrencyData;
        int quantity = isAlternate ? 5 : 1;
        if (currencyData.TryPurchase(shopItem.ShopItemData.ItemData, quantity))
        {
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
    
    public void OnSlotLeftClicked(ItemContainer itemContainer, Slot slot)
    {
        if (!transactionSlot.IsEmpty())
        {
            AddTransactionItemToSlot(slot);
        }
    }

    public void OnSlotRightClicked(ItemContainer itemContainer, Slot slot)
    {
        if (slot.IsEmpty()) return;
        
        // TODO check if the item category / item is sellable
        if (slot.ItemData.Category == ItemCategory.Produce)
        {
            SlotData slotData = itemContainer.TakeFromSlot(slot);
            int gold = slotData.ItemData.GoldCost * slotData.Quantity;
            GameManager.GameData.PlayerData.CurrencyData.AddGold(gold);
        }
        else
        {
            GD.Print($"Cannot sell {slot.ItemData.GetName()} here.");
        }
    }

    private void AddTransactionItemToSlot(Slot slot)
    {
        int remainingQuantity = slot.AddItem(transactionSlot.ItemData, transactionSlot.SlotData.Quantity);
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