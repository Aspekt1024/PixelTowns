#if TOOLS

using Godot;
using PixelTowns.Translation;

namespace PixelTowns;

[Tool]
public partial class PixelTownsPlugin : EditorPlugin
{
	private TranslationKeyPlugin plugin;
	
	public override void _EnterTree()
	{
		plugin = new TranslationKeyPlugin();
		AddInspectorPlugin(plugin);
	}

	public override void _ExitTree()
	{
		RemoveInspectorPlugin(plugin);
	}
}
#endif
