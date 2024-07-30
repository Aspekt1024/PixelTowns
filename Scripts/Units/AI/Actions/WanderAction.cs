using Godot;
using PixelTowns.World;

namespace PixelTowns.Units;

public class WanderAction : AiAction
{
    private float timeLastWandered;
    
    private OverTimeEffect useEnergyEffect;
    
    public WanderAction(Unit unit) : base(unit)
    {
    }

    protected override void Begin()
    {
        Vector2 destination = WorldUtil.RandomPathableCellInRadius(Unit.Position, 2);
        Unit.Movement.MoveTo(destination);
        Unit.Movement.DestinationReached += OnDestinationReached;

        useEnergyEffect = new OverTimeEffect(Unit.Stats.Energy, -1f, -1f);
        Unit.Stats.AddEffect(useEnergyEffect);
    }

    public override void Tick(float deltaTime)
    {
    }

    public override float GetUtility()
    {
        const float wanderInterval = 1000f;
        return ((Time.GetTicksMsec() - timeLastWandered) / wanderInterval) * Unit.Stats.Energy.Ratio;
    }

    private void OnDestinationReached()
    {
        timeLastWandered = Time.GetTicksMsec();
        UnregisterEvents();
        
        Unit.Stats.RemoveEffect(useEnergyEffect);
        useEnergyEffect = null;
        
        CompleteAction();
    }

    private void UnregisterEvents()
    {
        Unit.Movement.DestinationReached -= OnDestinationReached;
    }
}