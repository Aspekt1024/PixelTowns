using Godot;

namespace PixelTowns.UI;

public partial class UiBase : Control
{
    public virtual void Open()
    {
        Show();
    }

    public virtual void Close()
    {
        Hide();
    }
}