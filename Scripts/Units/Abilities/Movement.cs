using System;
using Godot;

namespace PixelTowns.Units;

public class Movement
{
    private readonly CharacterBody2D body;
    private readonly NavigationAgent2D navAgent;
    private readonly MovementSettings settings;

    private bool isFacingRight;

    public event Action DestinationReached = delegate { };
    public event Action<bool> MoveStateChanged = delegate { };
    public event Action<bool> DirectionChanged = delegate { };
    
    public Movement(CharacterBody2D body, NavigationAgent2D navAgent, MovementSettings settings)
    {
        this.body = body;
        this.navAgent = navAgent;
        this.settings = settings;

        navAgent.PathDesiredDistance = 1f;
        navAgent.TargetDesiredDistance = 1f;
        
        navAgent.NavigationFinished += OnDestinationReached;

        Callable.From(ActorSetup).CallDeferred();
    }

    ~Movement()
    {
        navAgent.NavigationFinished -= OnDestinationReached;
    }

    public void MoveTo(Vector2 location)
    {
        navAgent.TargetPosition = location;
        MoveStateChanged?.Invoke(true);
    }

    public void PhysicsTick(float deltaTime)
    {
        if (!navAgent.IsNavigationFinished())
        {
            ActionMovement(deltaTime);
        }
    }

    private void ActionMovement(float deltaTime)
    {
        Vector2 dist = navAgent.GetNextPathPosition() - body.GlobalPosition;
        Vector2 moveVector = dist.Normalized() * settings.moveSpeed * deltaTime;
        
        bool isMovingRight = moveVector.X > 0;
        if (isMovingRight != isFacingRight)
        {
            isFacingRight = isMovingRight;
            DirectionChanged?.Invoke(isFacingRight);
        }
        
        if (moveVector.Length() >= dist.Length())
        {
            body.MoveAndCollide(dist);
            DestinationReached?.Invoke();
        }
        else
        {
            body.MoveAndCollide(moveVector);
        }
    }

    private void OnDestinationReached()
    {
        DestinationReached?.Invoke();
        MoveStateChanged?.Invoke(false);
    }

    private async void ActorSetup()
    {
        await body.ToSignal(body.GetTree(), SceneTree.SignalName.PhysicsFrame);
    }
}