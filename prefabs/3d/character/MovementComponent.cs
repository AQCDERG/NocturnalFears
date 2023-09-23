using Godot;

namespace JayStation.prefabs._3d.character;

public partial class MovementComponent : Node3D {
	[Export] int speed = 5;
	public Vector3 velocity = Vector3.Zero;
	bool startMovement;

	public void BeginMovement() {
		SetProcessInput(true);
	}

	public void StopMovement() {
		SetProcessInput(false);
	}

	public override void _Input(InputEvent @event) {
		if (@event.IsActionPressed("movement_forward")) velocity.Z = -speed;
		if (@event.IsActionReleased("movement_forward")) velocity.Z = 0;

		if (@event.IsActionPressed("movement_backward")) velocity.Z = speed;
		if (@event.IsActionReleased("movement_backward")) velocity.Z = 0;

		if (@event.IsActionPressed("movement_right")) velocity.X = speed;
		if (@event.IsActionReleased("movement_right")) velocity.X = 0;

		if (@event.IsActionPressed("movement_left")) velocity.X = -speed;
		if (@event.IsActionReleased("movement_left")) velocity.X = 0;
	}
}