using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

namespace PixelTowns.Units;

[GlobalClass]
public partial class UnitAnimator : AnimationPlayer
{
    [Export] private Sprite2D spriteNode;
    [Export] private bool initiallyFacingRight;
    
    [Export] public Dictionary IdleAnims = new() { {"Idle", 1} };
    [Export] public StringName WalkAnim = "Walk";

    private enum State
    {
        Idle = 0,
        Walking = 1000,
    }

    private State state;

    private readonly List<StringName> overrides = new();

    public override void _Ready()
    {
        AnimationFinished += OnAnimationFinished;

        state = State.Idle;
        ResetAnimation();
    }

    public void Setup(Movement movement)
    {
        movement.DirectionChanged += OnDirectionChanged;
        movement.MoveStateChanged += OnMoveStateChanged;
    }

    public void AddOverride(StringName animName)
    {
        if (!overrides.Contains(animName))
        {
            overrides.Add(animName);   
        }
        
        Play(animName);
    }

    public void RemoveOverride(StringName animName)
    {
        overrides.Remove(animName);
    }

    private void ResetAnimation()
    {
        switch (state)
        {
            case State.Idle:
                SetIdle();
                break;
            case State.Walking:
                SetWalking();
                break;
        }
    }
    
    private void SetIdle()
    {
        state = State.Idle;

        if (overrides.Any()) return;
        
        float totalPool = IdleAnims.Sum(a => (float)a.Value);
        float selector = Random.Range(0, totalPool);
        float count = 0;
        
        foreach (var anim in IdleAnims)
        {
            float weight = (float)anim.Value;
            count += weight;
            if (count >= selector)
            {
                Play((StringName)anim.Key);
                break;
            }
        }
    }

    private void SetWalking()
    {
        state = State.Walking;
        if (!overrides.Any())
        {
            Play(WalkAnim);
        }
    }

    private void OnDirectionChanged(bool isFacingRight)
    {
        spriteNode.FlipH = !initiallyFacingRight && isFacingRight;
    }

    private void OnMoveStateChanged(bool isMoving)
    {
        if (isMoving)
        {
            SetWalking();
        }
        else
        {
            SetIdle();
        }
    }

    private void OnAnimationFinished(StringName stringName)
    {
        if (state == State.Idle)
        {
            SetIdle();
        }
    }
}