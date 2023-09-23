using System;
using System.Threading.Tasks;
using Godot;
using GodotExtensions.extensions.node;

namespace JayStation.scripts.Attacks;

public partial class RapidFire : AttackAction {
	[Export] public PackedScene bullet;

	[Export] public float pitchSound;

	public override void StartAttack() {
		BeginShooting();
		attackSound.PitchScale = pitchSound;
	}

	public void ShootBullet() {
		Random rnd = new();
		int index = rnd.Next(-2, 2);
		Bullet bullet = CreateBullet();
		attackSound.Playing = true;
		bullet.targetPosition = currentTarget.Position + new Vector3(index, 1, index);
		bullet.OnCreated();
	}

	public async Task BeginShooting() {
		for (int i = 0; i < (difficultyLevel * 5); i++) {
			await this.YieldForSeconds(0.2f);
			ShootBullet();
		}
	}

	public Bullet CreateBullet() {
		Bullet bulletInstance = bullet.Instantiate<Bullet>();
		bulletInstance.GlobalTransform = GlobalTransform;
		GetTree().Root.AddChild(bulletInstance);
		return bulletInstance;
	}
}