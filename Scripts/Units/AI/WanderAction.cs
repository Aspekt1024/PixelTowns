using Godot;

namespace PixelTowns.Units;

public class WanderAction : AiAction
{
    private float startTime;

    protected override void Begin()
    {
        startTime = Time.GetTicksMsec();
    }

    public override void Tick(double deltaTime)
    {
        if (Time.GetTicksMsec() > startTime + 2000f)
        {
            CompleteAction();
        }
    }

    public override float GetUtility()
    {
        return 1f;
    }
}