using Godot;
using PixelTowns.World;

namespace PixelTowns.Units;

public class WanderAction : AiAction
{
    private float timeLastWandered;
    
    public WanderAction(Unit unit) : base(unit)
    {
    }

    protected override void Begin()
    {
        Vector2 destination = WorldUtil.RandomCellInRadius(Unit.Position, 2);
        Unit.Movement.MoveTo(destination);
        Unit.Movement.DestinationReached += OnDestinationReached;
    }

    public override void Tick(float deltaTime)
    {
        
    }

    public override float GetUtility()
    {
        const float wanderInterval = 3000f;
        return (Time.GetTicksMsec() - timeLastWandered) / wanderInterval;
    }

    private void OnDestinationReached()
    {
        timeLastWandered = Time.GetTicksMsec();
        UnregisterEvents();
        CompleteAction();
    }

    private void UnregisterEvents()
    {
        Unit.Movement.DestinationReached -= OnDestinationReached;
    }
}