﻿using System;
using Godot;
using PixelTowns.Units;

namespace PixelTowns.Scripts.Units.AI;

public class RestAction : AiAction
{
    private float startTime;
    private const float RestDuration = 5000f;

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
        if (GameManager.Time.IsWithinTime(22f, 5.5f))
        {
            return 100000f; // TODO find a better way to override
        }
        return 1f - Unit.Stats.Energy.Ratio; // TODO add curves
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