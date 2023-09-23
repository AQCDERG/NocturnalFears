using Godot;
using JayStation.components._3d;
using JayStation.components.ui;
using JayStation.scripts.globals.autoloads;

namespace JayStation.prefabs._3d.Enemies;

public partial class Villians : CharacterBody3D {
	[Export] float speed = 5f;
	public float gravity = 6f;
	DetectionComponent detection;
	DetectionComponent attackArea;
	TargetingComponent targetingComponent;
	SwitchSceneComponenet switchSceneComponent;
	Vector3 velocity = Vector3.Zero;

	public override void _Ready() {
		switchSceneComponent = GetNode<SwitchSceneComponenet>("SwitchSceneComponent");
		detection = GetNode<DetectionComponent>("%DetectionComponent");
		targetingComponent = GetNode<TargetingComponent>("%TargetingComponent");
		attackArea = GetNode<DetectionComponent>("AttackArea");
		detection.ValidBodyEntered += Target;
		detection.ValidBodyExited += RemoveTarget;
		attackArea.ValidBodyEntered += Attack;
	}

	public void Target(CharacterBody3D target) {
		targetingComponent.SetTarget(target);
	}

	public void RemoveTarget() {
		targetingComponent.RemoveTarget();
	}

	public void Attack(CharacterBody3D target) {
		GameState.Instance.TriggerdVillian = GetType();
		switchSceneComponent.ChangeScene();
	}

	public override void _Process(double delta) {
		Velocity = Velocity with { Y = Velocity.Y - gravity * (float)delta };
		MoveAndSlide();
		if (targetingComponent.DoesNotHaveTarget) return;
		targetingComponent.updateTarget();
		Vector3 nextLocation = targetingComponent.GetNextPathPosition();
		Vector3 VelocityToTarget = (nextLocation - GlobalPosition).Normalized();
		//velocity = new Vector3(GlobalPosition.X, -gravity, GlobalPosition.Z);
		Velocity = VelocityToTarget * speed;
		GD.Print("NOO");
	}
}