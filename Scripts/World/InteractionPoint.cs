using Godot;

namespace PixelTowns.World;

public abstract partial class InteractionPoint : Area2D
{
    public override void _Ready()
    {
        InputEvent += OnInput;
    }

    private void OnInput(Node viewport, InputEvent @event, long shapeidx)
    {
        if (InputManager.IsMouseInteract(@event))
        {
            HandleInteraction();
        }
    }

    protected abstract void HandleInteraction();
}