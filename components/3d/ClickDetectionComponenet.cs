using Godot;

public partial class ClickDetectionComponenet : Area3D {
	//[Signal]
	//public delegate void OnItemHoveredEventHandler(Item item);
	//public delegate void onItemStopHoveringEventHandler(Item item);

	Node3D target;
	public bool hovering;
	bool grabbed;

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
}