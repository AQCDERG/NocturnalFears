using Godot;

namespace JayStation.prefabs.other.player;

public partial class Player : Node {
	public Vector3 velocity = Vector3.Zero;
	int speed = 5;

	public void beginMovement() {
		SetProcessInput(true);
	}

	public void endMovement() {
		SetProcessInput(false);
	}


	public override void _Input(InputEvent @event) {
		if (@event.IsActionPressed("movement_up")) velocity.X = speed;
		if (@event.IsActionPressed("movement_down")) velocity.X = -speed;
		if (@event.IsActionPressed("movement_right")) velocity.Z = speed;
		if (@event.IsActionPressed("movement_left")) velocity.Z = -speed;
		if (@event.IsActionPressed("movement_sprint")) speed *= 2;
	}
}