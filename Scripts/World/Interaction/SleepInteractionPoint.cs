using Godot;

namespace PixelTowns.World;

public partial class SleepInteractionPoint : InteractionPoint
{
    protected override void HandleInteraction()
    {
        GameManager.Time.ProgressDay();
    }
}