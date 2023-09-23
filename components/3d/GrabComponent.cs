using Godot;
using GodotExtensions.extensions.node;

namespace JayStation.components._3d;

public partial class GrabComponent : Area3D {
	Node3D target;
	bool hovering;
	bool grabbed;
	Timer reset;
	float resetTimer = 2;
	[Export] float weight;
	[Export] bool canReset;

	public override void _Ready() {
		target = GetParent<Node3D>();
		MouseEntered += Hovering;
		MouseExited += NotHovering;
	}

	public void Move(InputEvent @event) {
		if (!grabbed) return;
		InputEventMouseMotion mouseMotion = @event as InputEventMouseMotion;
		mouseMotion = @event as InputEventMouseMotion;
		target.Position = new Vector3(target.Position.X + mouseMotion.Relative.X * 0.01f, target.Position.Y,
			target.Position.Z);
	}

	public void Hovering() {
		hovering = true;
	}

	public void NotHovering() {
		hovering = false;
	}

	public override void _Input(InputEvent @event) {
		if (@event is InputEventMouseButton mouseButtonEvent) {
			if (mouseButtonEvent.Pressed)
				grabbed = true;
			else if (!mouseButtonEvent.Pressed) {
				grabbed = false;
				if (canReset) resetPosition();
			}
		}

		Move(@event);
	}


	public async void resetPosition() {
		this.YieldForSeconds(2);
		target.Position = new Vector3(0, target.Position.Y, target.Position.Z);
	}
}