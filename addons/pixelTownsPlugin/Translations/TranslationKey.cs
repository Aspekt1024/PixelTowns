using Godot;

namespace PixelTowns.Translation;

#if TOOLS
[Tool]
#endif
[GlobalClass]
public partial class TranslationKey : Resource
{
    [Export] public string Key;

    public string GetTranslation() => Tr(Key);
}