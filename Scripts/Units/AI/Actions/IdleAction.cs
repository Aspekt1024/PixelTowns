using Godot;
using PixelTowns.Units;

namespace PixelTowns.Scripts.Units.AI;

public class IdleAction : AiAction
{
    private float startTime;
    private const float IdleDuration = 2000f;
    
    public IdleAction(Unit unit) : base(unit)
    {
    }

    public override void Tick(float deltaTime)
    {
        if (Time.GetTicksMsec() > startTime + IdleDuration)
        {
            CompleteAction();
        }
    }

    public override float GetUtility()
    {
        return 0.4f;
    }

    protected override void Begin()
    {
        startTime = Time.GetTicksMsec();
    }
}