using Godot;

namespace PixelTowns.UI;

public partial class HUD : Control, TimeData.IObserver
{
    [Export] private Label dayLabel;
    [Export] private Label timeLabel;
    [Export] private Label moneyLabel;

    public void Init(GameData data)
    {
        data.TimeData.RegisterObserver(this);
    }

    public void OnDayChanged(int day)
    {
        dayLabel.Text = $"Day {day}";
    }

    public void OnTimeChanged(float normalisedTime)
    {
        timeLabel.Text = TimeSettings.GetTimeString(normalisedTime);
    }
}