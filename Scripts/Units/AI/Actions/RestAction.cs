using System;
using Godot;
using PixelTowns.Units;

namespace PixelTowns.Scripts.Units.AI;

public class RestAction : AiAction
{
    private float startTime;
    private const float RestDuration = 7000f;

    private OverTimeEffect overTimeEffect;
    private readonly Action<bool> onRestCallback;

    public RestAction(Unit unit, Action<bool> onRestCallback) : base(unit)
    {
        this.onRestCallback = onRestCallback;
    }

    public override void Tick(float deltaTime)
    {
        if (Time.GetTicksMsec() > startTime + RestDuration)
        {
            EndEffect();
            CompleteAction();
        }
    }

    public override float GetUtility()
    {
        return 1 - Unit.Stats.Energy.Ratio; // TODO add curves
    }

    protected override void Begin()
    {
        startTime = Time.GetTicksMsec();
        overTimeEffect = new OverTimeEffect(Unit.Stats.Energy, 1f, -1f);
        Unit.Stats.AddEffect(overTimeEffect);
        onRestCallback?.Invoke(true);
    }

    private void EndEffect()
    {
        onRestCallback?.Invoke(false);
        Unit.Stats.RemoveEffect(overTimeEffect);
        overTimeEffect = null;
    }
}