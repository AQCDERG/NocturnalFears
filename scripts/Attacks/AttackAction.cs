using Godot;
using JayStation.prefabs._3d.Enemies.BattleEnemies;

namespace JayStation.scripts.Attacks;

public partial class AttackAction : Node3D {
	public AudioStreamPlayer attackSound;
	public CharacterBody3D currentTarget;
	public GpuParticles3D hitParticle;
	public int difficultyLevel;

	public override void _Ready() {
		attackSound = GetNode<AudioStreamPlayer>("%AttackSound");
		difficultyLevel = GetOwner<BattleVillian>().diffcultyLevel;
		hitParticle = GetNode<GpuParticles3D>("%TargetHit");
	}

	public virtual void StartAttack() { }

	public virtual void StopAttack() { }
}