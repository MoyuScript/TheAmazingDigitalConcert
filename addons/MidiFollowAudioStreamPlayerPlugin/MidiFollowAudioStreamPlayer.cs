using Godot;
using Commons.Music.Midi;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

public partial class MidiFollowAudioStreamPlayer : Node
{
    [Export] public AudioStreamPlayer AudioPlayer;
    [Export(PropertyHint.File)] public string MidiPath;

    [Signal] public delegate void ReceivedMidiEventEventHandler();

    public MidiEvent CurrentMidiEvent { get; private set; }
    MidiMusic _midiMusic;
    List<Commons.Music.Midi.MidiMessage> _mergedMidiMessages;

    public override void _EnterTree()
    {
    }


    public override void _Ready()
    {
        _midiMusic = MidiMusic.Read(System.IO.File.OpenRead(ProjectSettings.GlobalizePath(MidiPath)));

        _mergedMidiMessages = new List<Commons.Music.Midi.MidiMessage>();

        foreach (var track in _midiMusic.Tracks)
        {
            int delta = 0;
            foreach (var message in track.Messages)
            {
                delta += message.DeltaTime;
                _mergedMidiMessages.Add(new Commons.Music.Midi.MidiMessage(delta, message.Event));
            }
        }

        _mergedMidiMessages.Sort((Commons.Music.Midi.MidiMessage a, Commons.Music.Midi.MidiMessage b) => a.DeltaTime - b.DeltaTime);
    }

    int _currentMidiEventIndex = 0;
    int _currentTempo = 500000;
    int _midiCurrentPosition = 0;
    public override void _Process(double delta)
    {
        int currentAudioPosition = (int)(AudioPlayer.GetPlaybackPosition() * 1000);
        while (_midiCurrentPosition <= currentAudioPosition)
        {
            if (_currentMidiEventIndex >= _mergedMidiMessages.Count)
            {
                break;
            }
            var currentMidiMessage = _mergedMidiMessages[_currentMidiEventIndex];
            _midiCurrentPosition = (int)((float)currentMidiMessage.DeltaTime / _midiMusic.DeltaTimeSpec * (_currentTempo / 1000));

            if (_midiCurrentPosition > currentAudioPosition) break;
            
            if (currentMidiMessage.Event.StatusByte == 0xff)
            {
                if (currentMidiMessage.Event.Msb == MidiMetaType.Tempo)
                {
                    _currentTempo = MidiMetaType.GetTempo(currentMidiMessage.Event.ExtraData, currentMidiMessage.Event.ExtraDataOffset);
                }
            }

            CurrentMidiEvent = currentMidiMessage.Event;
            EmitSignal(SignalName.ReceivedMidiEvent);
            _currentMidiEventIndex++;
        }
    }
}