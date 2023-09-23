using System.Threading.Tasks;
using Godot;
using GodotExtensions.extensions.node;
using JayStation.components._3d;
using JayStation.prefabs._3d.character;

public partial class Bullet : RigidBody3D {
	public Vector3 targetPosition;
	DetectionComponent detectionComponent;

	public override void _Ready() {
		detectionComponent = GetNode<DetectionComponent>("%DetectionComponent");
		detectionComponent.ValidBodyEntered += OnHit;
	}

	public void OnCreated() {
		LinearVelocity = targetPosition - GlobalPosition;
		NotHit();
	}

	public void OnHit(CharacterBody3D target) {
		if (target is Character character) {
			character.health.TakeDamage(20);
			deleteBullet();
		}
	}

	public async Task NotHit() {
		await this.YieldForSeconds(6);
		deleteBullet();
	}

	public async void deleteBullet() {
		QueueFree();
	}
}