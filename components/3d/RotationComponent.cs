using Godot;

namespace JayStation.components._3d;

[Tool]
public partial class RotationComponent : Node3D {
	[Export] float rotationSpeed = 1;
	[Export] Node3D target;

	public override void _Process(double delta) {
		float rotation = target.Rotation.Y + rotationSpeed * (float)delta;
		target.Rotation = target.Rotation with { Y = rotation };
	}
}