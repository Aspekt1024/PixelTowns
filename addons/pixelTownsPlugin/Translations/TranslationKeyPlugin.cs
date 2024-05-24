#if TOOLS

using System.Collections.Generic;
using System.Linq;
using Godot;

namespace PixelTowns.Translation;

[Tool]
public partial class TranslationKeyPlugin : EditorInspectorPlugin
{
    private List<string> translationKeys;
    
    public override bool _CanHandle(GodotObject @object)
    {
        return true;
    }

    public override bool _ParseProperty(GodotObject @object, Variant.Type type, string name, PropertyHint hintType, string hintString, PropertyUsageFlags usageFlags, bool wide)
    {
        if (@object?.Get(name).Obj is TranslationKey tk)
        {
            if (translationKeys == null || !translationKeys.Any())
            {
                LoadKeys();
            }

            var editorProp = new TranslationKeyEditorProperty(name, tk, translationKeys);
            AddCustomControl(editorProp);
            return true;
        }
        return false;
    }

    private void LoadKeys()
    {
        translationKeys = new List<string>();
        
        Godot.Translation translation = ResourceLoader.Load<Godot.Translation>("res://Data/Localisations/Items.en.translation");
        translationKeys = translationKeys.Concat(translation.GetMessageList()).ToList();
    }
}

#endif
