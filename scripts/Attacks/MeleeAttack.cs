using Godot;
using JayStation.components._3d;
using JayStation.prefabs._3d.character;
using JayStation.prefabs._3d.Enemies.BattleEnemies;

namespace JayStation.scripts.Attacks;

public partial class MeleeAttack : AttackAction {
	BattleVillian parent;
	DetectionComponent attakArea;
	public TargetingComponent targetingComponent;

	public override void _Ready() {
		attakArea = GetNode<DetectionComponent>("%AttackArea");
		parent = Owner as BattleVillian;
		targetingComponent = GetNode<TargetingComponent>("%TargetingComponent");
	}

	public override void StartAttack() {
		Chase(currentTarget);
		attakArea.ValidBodyEntered += Attack;
	}

	public void Chase(CharacterBody3D target) {
		GD.Print("HAS TARGET");
		targetingComponent.SetTarget(target);
	}

	public void Attack(CharacterBody3D target) {
		if (target is Character character) {
			hitParticle.Emitting = true;
			character.health.TakeDamage(parent.diffcultyLevel);
		}
	}

	public override void StopAttack() {
		targetingComponent.RemoveTarget();
		parent.Velocity = Vector3.Zero;
	}

	public override void _Process(double delta) {
		base._Process(delta);
		if (targetingComponent.DoesNotHaveTarget) return;
		targetingComponent.updateTarget();
		Vector3 nextLocation = targetingComponent.GetNextPathPosition();
		Vector3 VelocityToTarget = (nextLocation - GlobalPosition).Normalized();
		parent.Velocity = VelocityToTarget * parent.diffcultyLevel;
	}
}