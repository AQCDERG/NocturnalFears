using Godot;
using JayStation.components;

namespace JayStation.globals.components.Cooldown;

[GlobalClass]
public partial class CooldownComponent : GlobalComponent {
	[Signal]
	public delegate void OnCooldownEndedEventHandler();

	[Export] public double CooldownTimeSeconds { get; set; }
	public double ElapsedTimeSeconds { get; private set; }
	public bool IsCooldownOver => ElapsedTimeSeconds >= CooldownTimeSeconds;
	public bool IsOnCooldown => !IsCooldownOver;

	public void StartCooldown() {
		ElapsedTimeSeconds = 0;
	}

	public override void _EnterTree() {
		// Reset cooldown/dont start automatically
		ElapsedTimeSeconds = CooldownTimeSeconds;
	}

	public override void _Process(double delta) {
		if (IsCooldownOver)
			return;

		ElapsedTimeSeconds += delta;

		if (IsCooldownOver)
			EmitSignal(SignalName.OnCooldownEnded);
	}
}