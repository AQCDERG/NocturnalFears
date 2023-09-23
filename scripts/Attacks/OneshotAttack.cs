using System.Threading.Tasks;
using Godot;
using GodotExtensions.extensions.node;

namespace JayStation.scripts.Attacks;

public partial class OneshotAttack : AttackAction {
	[Export] public PackedScene bullet;
	[Export] int bulletAmount;
	[Export] float pitchSound;

	public override void StartAttack() {
		BeginShooting(currentTarget);
	}

	public async Task BeginShooting(CharacterBody3D target) {
		for (int i = 0; i < (bulletAmount * difficultyLevel); i++) {
			await this.YieldForSeconds(1);
			ShootBullet(target);
			attackSound.PitchScale = pitchSound;
		}
	}

	public void ShootBullet(CharacterBody3D target) {
		Bullet bullet = CreateBullet();
		bullet.targetPosition = target.Position;
		attackSound.Playing = true;
		bullet.OnCreated();
	}

	public Bullet CreateBullet() {
		Bullet bulletInstance = bullet.Instantiate<Bullet>();
		bulletInstance.GlobalTransform = GlobalTransform;
		GetTree().Root.AddChild(bulletInstance);
		return bulletInstance;
	}
}