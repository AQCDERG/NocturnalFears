using Godot;

namespace JayStation.prefabs._3d.Enemies.BattleEnemies;

public partial class BattleVillian : CharacterBody3D {
	public AudioStreamPlayer themeSong;
	int gravity = 10;
	[Export] public int diffcultyLevel;

	public override void _Ready() {
		themeSong = GetNode<AudioStreamPlayer>("Theme");
		SetProcess(false);
	}

	public override void _Process(double delta) {
		Velocity = Velocity with { Y = Velocity.Y - gravity * (float)delta };
		MoveAndSlide();
	}
}