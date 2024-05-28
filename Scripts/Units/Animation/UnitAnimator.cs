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

    public override void _Ready()
    {
        AnimationFinished += OnAnimationFinished;
        
        SetIdle();
    }

    public void Setup(Movement movement)
    {
        movement.DirectionChanged += OnDirectionChanged;
        movement.MoveStateChanged += OnMoveStateChanged;
    }
    
    private void SetIdle()
    {
        state = State.Idle;
        
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
        Play(WalkAnim);
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