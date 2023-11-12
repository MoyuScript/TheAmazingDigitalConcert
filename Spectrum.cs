using Godot;
using System.Collections.Generic;
using System.Numerics;

public partial class Spectrum : Node3D
{
	[Export] Material BarMaterial;
	const int VU_COUNT = 32;
	const int FREQ_MIN = 20;
	const int FREQ_MAX = 1000;
	const float HEIGHT_MIN = 0.05f;
	const float HEIGHT_MAX = 3;
	const float WIDTH = 9;
	const float BAR_DEPTH = 0.05f;
	const float BAR_GAP = 0.05f;
	const float HEIGHT_BOOST = 4.0f;
	const float SAMPLE_DELAY = 0.1f;

	AudioEffectSpectrumAnalyzerInstance _spectrum;
	List<(int freq, MeshInstance3D meshInstance3D)> _freqMeshList = new();

	public override void _Ready()
	{
		_spectrum = (AudioEffectSpectrumAnalyzerInstance)AudioServer.GetBusEffectInstance(0, 0);

		int freqStep = (FREQ_MAX - FREQ_MIN) / VU_COUNT;
		float barWidth = WIDTH / VU_COUNT - BAR_GAP;
		BoxMesh boxMesh = new()
		{
			Material = BarMaterial,
		};
		for (int i = 0; i < VU_COUNT; i++)
		{
			float x = i * (barWidth + BAR_GAP) - WIDTH / 2;
			int freq = i * freqStep + FREQ_MIN;
			MeshInstance3D meshInstance3D = new()
			{
				Mesh = boxMesh,
				Position = new Godot.Vector3(x, HEIGHT_MIN / 2, 0),
				Scale = new Godot.Vector3(barWidth, HEIGHT_MIN, BAR_DEPTH),
				CastShadow = GeometryInstance3D.ShadowCastingSetting.On
			};
			AddChild(meshInstance3D);
			_freqMeshList.Add((freq, meshInstance3D));
		}
	}

	float _deltaSum;
	public override void _Process(double delta)
	{
		_deltaSum += (float)delta;
		if (_deltaSum < SAMPLE_DELAY) return;
		_deltaSum = 0;

		int freqStep = (FREQ_MAX - FREQ_MIN) / VU_COUNT;
		foreach (var (freq, meshInstance3D) in _freqMeshList)
		{
			var magnitude = _spectrum.GetMagnitudeForFrequencyRange(freq, freq + freqStep).Length();
			float height = Mathf.Clamp((HEIGHT_MAX - HEIGHT_MIN) * magnitude * HEIGHT_BOOST + HEIGHT_MIN, HEIGHT_MIN, HEIGHT_MAX);

			var tween = meshInstance3D.CreateTween();
			tween.Parallel().TweenProperty(meshInstance3D, "scale:y", height, SAMPLE_DELAY);
			tween.Parallel().TweenProperty(meshInstance3D, "position:y", height / 2, SAMPLE_DELAY);
		}
	}
}
