﻿using Godot;
using PixelTowns.Units.AI;

namespace PixelTowns.Units;

public abstract partial class Unit : CharacterBody2D
{
    [Export] public UnitStats Stats;
    [Export] protected UnitAnimator Animator;
    [Export] private MovementSettings movementSettings;
    [Export] private NavigationAgent2D navAgent;
    
    public readonly AiEngine Ai = new ();
    public Movement Movement { get; private set; }

    public override void _Ready()
    {
        Movement = new Movement(this, navAgent, movementSettings);
        Animator.Setup(Movement);
        Stats = (UnitStats)Stats.Duplicate(true);
        
        Init();
    }

    protected abstract void Init();

    public override void _Process(double delta)
    {
        float deltaTime = (float)delta;
        
        Stats.Tick(deltaTime);
        Ai.Tick(deltaTime);
    }

    public override void _PhysicsProcess(double delta)
    {
        float deltaTime = (float)delta;
        Movement.PhysicsTick(deltaTime);
    }
}
