using Godot;
using System;
using Commons.Music.Midi;

public partial class Main : Node3D
{
	bool _isPlaying;
	public override void _Ready()
	{
		
	}

	public override void _Process(double delta)
	{
		if (!_isPlaying && Input.IsActionJustPressed("start"))
		{
			_isPlaying = true;
			Start();
		}
	}

	public void Start()
	{
		Characters characters = GetNode<Characters>("Stage/Characters");
		var path = ProjectSettings.GlobalizePath("res://percussions.mid");
		var music = MidiMusic.Read(System.IO.File.OpenRead(path));
		var player = new MidiPlayer(music);
		player.EventReceived += (MidiEvent e) =>
		{
			if (e.EventType == MidiEvent.NoteOn)
			{

				int channel = e.Value & 0xf;
				int noteNumber = e.Value >> 8 & 0xff;

				if (channel != 9) return;

				if (noteNumber == 38)
				{
					// 军鼓
					characters.CallDeferred("HitSnare");
				}
				else if (noteNumber == 36)
				{
					// 定音鼓
					characters.CallDeferred("HitTimpani");
				}
				else if (noteNumber == 54)
				{
					// 铃鼓
					characters.CallDeferred("HitTambourine");
				}
			}
			else if (e.EventType == MidiEvent.NoteOff)
			{
				int channel = e.Value & 0xf;
				int noteNumber = e.Value >> 8 & 0xff;

				if (channel != 9) return;

				if (noteNumber == 38)
				{
					// 军鼓
					characters.CallDeferred("IdleSnare");
				}
				else if (noteNumber == 36)
				{
					// 定音鼓
					characters.CallDeferred("IdleTimpani");
				}
				else if (noteNumber == 54)
				{
					// 铃鼓
					characters.CallDeferred("IdleTambourine");
				}
			}
		};
		player.Play();

		GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
		GetNode<AnimationPlayer>("PerformAnimationPlayer").Play("perform");
		GetNode<AnimationPlayer>("Stage/Logo/AnimationPlayer").Play("beating");
	}
}
