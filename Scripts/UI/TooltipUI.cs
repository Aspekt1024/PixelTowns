using Godot;

namespace PixelTowns.UI;

public partial class TooltipUI : Control
{
    [Export] private Label titleLabel;
    [Export] private Label categoryLabel;
    [Export] private Label description;
    [Export] private RichTextLabel currency;
}