using Godot;
using PixelTowns.Items;

namespace PixelTowns.UI;

public partial class HUD : Control, TimeData.IObserver, CurrencyData.IObserver
{
    [Export] private Label dayLabel;
    [Export] private Label timeLabel;
    [Export] private Label moneyLabel;

    public void Init(GameData data)
    {
        data.TimeData.RegisterObserver(this);
        data.PlayerData.CurrencyData.RegisterObserver(this);
        
        OnDayChanged(data.TimeData.Day, 0);
        OnTimeChanged(data.TimeData.NormalisedTime);
        OnCurrencyModified(data.PlayerData.CurrencyData);
    }

    public void OnDayChanged(int newDay, int daysIncremented)
    {
        dayLabel.Text = $"Day {newDay}";
    }

    public void OnTimeChanged(float normalisedTime)
    {
        timeLabel.Text = TimeSettings.GetTimeString(normalisedTime);
    }

    public void OnCurrencyModified(CurrencyData currencyData)
    {
        moneyLabel.Text = currencyData.Gold + " g";
    }
}