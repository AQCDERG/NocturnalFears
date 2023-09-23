using Godot;
using GodotExtensions.scripts.utils;

namespace JayStation.components.other;

[GlobalClass]
public partial class RandomOffsetComponent : Node {
	[Export] public NoiseTexture2D NoiseTexture { get; set; }
	[Export] public float Magnitude { get; set; } = 0.1f;
	[Export] public float Speed { get; set; } = 1f;

	public Vector2 CurrentOffset { get; private set; }
	double time;
	Log log;

	public override void _Ready() {
		if (NoiseTexture?.Noise == null) {
			GD.PrintErr("NoiseTexture is not set or invalid.");
		}
	}

	public override void _Process(double delta) {
		if (NoiseTexture?.Noise == null)
			return;

		time += delta * Speed;

		float noiseValueX = NoiseTexture.Noise.GetNoise2D(0, (float)time);
		float noiseValueY = NoiseTexture.Noise.GetNoise2D((float)time, 0);

		noiseValueX = noiseValueX * Magnitude;
		noiseValueY = noiseValueY * Magnitude;

		CurrentOffset = new Vector2(
			noiseValueX,
			noiseValueY
		);
	}
}