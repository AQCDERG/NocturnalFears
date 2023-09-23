using Godot;

namespace JayStation.components._3d;

public partial class ClickDetection : Area3D {
	//[Signal]
	//public delegate void OnClick(InputEvent @event);

	bool hovering;

	public bool grabbed;

	public override void _Ready() {
		MouseEntered += Hovering;
		MouseExited += NotHovering;
	}

	public void Hovering() {
		hovering = true;
	}

	public void NotHovering() {
		hovering = false;
	}

	public override void _Input(InputEvent @event) {
		if (@event is InputEventMouseButton && hovering)
			grabbed = true;
		//EmitSignal(nameof(OnClick), @event);
		else if (!hovering) grabbed = false;
	}
}