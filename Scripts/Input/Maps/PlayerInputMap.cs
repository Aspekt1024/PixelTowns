using System.Collections.Generic;
using Godot;

namespace PixelTowns;

public class PlayerActionsInputMap : GameInputMap
{
    private const string MoveLeft = "moveLeft";
    private const string MoveRight = "moveRight";
    private const string MoveUp = "moveUp";
    private const string MoveDown = "moveDown";
    
    private const string Interact = "interact";
    private const string Use = "use";
    
    public interface IObserver
    {
        void OnInteractPressed();
        void OnUsePressed();
    }

    private readonly List<IObserver> observers = new();

    public void RegisterObserver(IObserver observer) => observers.Add(observer);
    public void UnregisterObserver(IObserver observer) => observers.Remove(observer);
    
    private static readonly List<string> PlayerMap = new()
    {
        MoveUp, MoveDown, MoveLeft, MoveRight,
        Interact, Use,
    };

    protected override List<string> Actions => PlayerMap;

    protected override void OnTick()
    {
        
    }

    public Vector2 GetMovementAxis()
    {
        if (!IsEnabled) return Vector2.Zero;
        
        Vector2 inputAxis = new Vector2()
        {
            X = Input.GetActionStrength(MoveRight) - Input.GetActionStrength(MoveLeft),
            Y = Input.GetActionStrength(MoveDown) - Input.GetActionStrength(MoveUp)
        };
        return inputAxis.Normalized();
    }
}