namespace PixelTowns;

public class GameTime
{
    private const float timePerDay = 1f;

    private double timeInDay = 0;
    private int day;

    public GameTime()
    {
        day = 0;
    }
    
    public void Tick(double delta)
    {
        timeInDay += delta;
        if (timeInDay >= timePerDay)
        {
            timeInDay -= timePerDay;
            day++;
            GameManager.IncrementDay(1);
        }
    }
}