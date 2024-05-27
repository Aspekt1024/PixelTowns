using System;
using Godot;

namespace PixelTowns.Units;

public class Movement
{
    private readonly CharacterBody2D body;
    private readonly MovementSettings settings;

    private Vector2 destination;

    public event Action DestinationReached = delegate { };

    public bool IsMoving { get; private set; }

    public Movement(CharacterBody2D body, MovementSettings settings)
    {
        this.body = body;
        this.settings = settings;
    }

    public void MoveTo(Vector2 location)
    {
        destination = location;
        IsMoving = true;
        // TODO find path using Astar2D and NavigationServer2D
    }

    public void Tick(float deltaTime)
    {
        if (IsMoving)
        {
            ActionMovement(deltaTime);
        }
    }

    private void ActionMovement(float deltaTime)
    {
        Vector2 dist = destination - body.Position;
        Vector2 direction = dist.Normalized();

        Vector2 moveVector = dist.Normalized() * settings.moveSpeed * deltaTime;
        if (moveVector.Length() >= dist.Length())
        {
            body.MoveAndCollide(dist);
            IsMoving = false;
            DestinationReached?.Invoke();
        }
        else
        {
            body.MoveAndCollide(moveVector);
        }
    }
}