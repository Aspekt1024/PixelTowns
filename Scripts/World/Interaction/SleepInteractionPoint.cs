using Godot;

namespace PixelTowns.World;

public partial class SleepInteractionPoint : InteractionPoint
{
    protected override void HandleInteraction()
    {
        GameManager.UI.Transition.FadeToBlack(() =>
        {
            GD.Print("next day!");
            GameManager.UI.Transition.FadeToScene(null);
        });
    }
}