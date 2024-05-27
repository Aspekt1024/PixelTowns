using Godot;
using PixelTowns.World;

namespace PixelTowns.Units;

public class WanderAction : AiAction
{
    public WanderAction(Unit unit) : base(unit)
    {
    }

    protected override void Begin()
    {
        Vector2 destination = WorldUtil.RandomCellInRadius(Unit.Position, 2);
        Unit.Movement.MoveTo(destination);
        Unit.Movement.DestinationReached += OnDestinationReached;
    }

    public override void Tick(double deltaTime)
    {
    }

    public override float GetUtility()
    {
        return 1f;
    }

    private void OnDestinationReached()
    {
        UnregisterEvents();
        CompleteAction();
    }

    private void UnregisterEvents()
    {
        Unit.Movement.DestinationReached -= OnDestinationReached;
    }
}