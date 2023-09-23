using Godot;

namespace JayStation.components._3d;

public partial class TargetingComponent : NavigationAgent3D {
	public Vector3 VelocityToTarget { get; private set; }
	public bool HasTarget => currentTarget != null;
	public bool DoesNotHaveTarget => currentTarget == null;
	CharacterBody3D parent;
	public CharacterBody3D currentTarget;

	public override void _EnterTree() {
		parent = Owner as CharacterBody3D;
	}

	public void SetTarget(CharacterBody3D target) {
		TargetPosition = target.Position;
		currentTarget = target;
	}

	public void RemoveTarget() {
		currentTarget = null;
	}

	public void updateTarget() {
		TargetPosition = currentTarget.Position;
	}

	public Vector3 GetTarget() => TargetPosition;
}