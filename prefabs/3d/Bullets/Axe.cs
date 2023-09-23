using Godot;

namespace JayStation.prefabs._3d.Bullets;

public partial class Axe : Bullet {
	Node3D moveablePart;
	float incrementalIncrease = 1.0f;

	public override void _Ready() {
		base._Ready();
		moveablePart = GetNode<Node3D>("MovableObject");
	}

	public override void _Process(double delta) {
		moveablePart.Position = moveablePart.Position with { X = LinearVelocity.X + incrementalIncrease / 25 };
		moveablePart.Rotation = Rotation with { Y = Rotation.Y + 8 * (float)delta };
		Rotation = Rotation with { Y = Rotation.Y + 4 * (float)delta };
		incrementalIncrease++;
	}
}