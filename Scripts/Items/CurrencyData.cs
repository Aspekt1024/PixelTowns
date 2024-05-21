using System.Collections.Generic;
using Godot;

namespace PixelTowns.Items;

[GlobalClass]
public partial class CurrencyData : Resource
{
    [Export] private int gold;
    [Export] private int soulCoins;
    
    internal interface IObserver
    {
        void OnCurrencyModified(CurrencyData currencyData);
    }

    private readonly List<IObserver> observers = new List<IObserver>();

    internal void RegisterObserver(IObserver o) => observers.Add(o);
    internal void UnregisterObserver(IObserver o) => observers.Remove(o);

    public int Gold
    {
        get => gold;
        private set
        {
            gold = value;
            observers.ForEach(o => o.OnCurrencyModified(this));
        }
    }
    
    public bool TryPurchase(ItemData itemData, int quantity)
    {
        int goldCost = itemData.GoldCost * quantity;
        if (Gold >= goldCost)
        {
            Gold -= goldCost;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddGold(int amount)
    {
        Gold += amount;
    }
}