using Godot;

namespace PixelTowns.Units;

public class OverTimeEffect : IStatEffect
{
    private readonly ResourceStat stat;
    private readonly float amountPerSecond;
    private float duration;
    
    public interface IObserver
    {
        void OnEffectExpired(IStatEffect effect);
    }
    private IObserver observer;
    
    public void RegisterSingleObserver(IObserver o) => observer = o;
    
    public OverTimeEffect(ResourceStat stat, float amountPerSecond, float duration)
    {
        this.stat = stat;
        this.amountPerSecond = amountPerSecond;
        this.duration = duration;
    }
    
    public void Tick(float deltaTime)
    {
        stat.Modify(amountPerSecond * deltaTime);
        
        if (duration > 0) // a duration of -1 (or any negative number) counts as infinite
        {
            duration -= deltaTime;
            if (duration <= 0)
            {
                observer?.OnEffectExpired(this);
            }
        }
    }
}