using Godot;
using PixelTowns.Units;

namespace PixelTowns.Scripts.Units.AI;

public class RestAction : AiAction
{
    private float startTime;
    private const float IdleDuration = 2000f;
    
    public RestAction(Unit unit) : base(unit)
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
        return 0.8f;
    }

    protected override void Begin()
    {
        startTime = Time.GetTicksMsec();
    }
}