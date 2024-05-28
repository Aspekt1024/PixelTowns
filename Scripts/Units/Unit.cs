using Godot;
using PixelTowns.Units.AI;

namespace PixelTowns.Units;

public abstract partial class Unit : CharacterBody2D
{
    [Export] private UnitAnimator unitAnimator;
    [Export] private MovementSettings movementSettings;
    
    public readonly AiEngine Ai = new ();
    public Movement Movement { get; private set; }

    protected virtual string IdleAnim => "Idle";
    protected virtual string WalkAnim => "Walk";

    public override void _Ready()
    {
        Movement = new Movement(this, movementSettings);
        unitAnimator.Setup(Movement);
        
        Init();
    }

    protected abstract void Init();

    public override void _Process(double delta)
    {
        Ai.Tick((float)delta);
        Movement.Tick((float)delta);
    }
}
