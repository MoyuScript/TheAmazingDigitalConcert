#if TOOLS
using Godot;
using System;

[Tool]
public partial class MidiFollowAudioStreamPlayerPlugin : EditorPlugin
{
	public override void _EnterTree()
	{
		var script = GD.Load<Script>("res://addons/MidiFollowAudioStreamPlayerPlugin/MidiFollowAudioStreamPlayer.cs");
		AddCustomType("MidiFollowAudioStreamPlayer", "Node", script, null);
	}

	public override void _ExitTree()
	{
		// Clean-up of the plugin goes here.
		RemoveCustomType("MidiFollowAudioStreamPlayer");
	}

}
#endif
