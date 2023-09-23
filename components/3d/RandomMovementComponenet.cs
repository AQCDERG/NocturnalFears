using Godot;

namespace JayStation.components._3d;

[GlobalClass]
public partial class RandomMovementComponenet : GlobalComponent {
	[Export] public float Max { get; set; }
	[Export] float Min { get; set; }
	[Export] Timer changeDirectionTimer;
	[Export] Node3D moveableObject;

	Vector3 velocity = Vector3.Zero;
	int incrementalIncrease = 2;
	float acceleration;
	Timer speedTimer;

	public override void _Ready() {
		changeDirectionTimer.Timeout += ChangeDirection;
		ConfigureTimer();
	}

	public void ConfigureTimer() {
		speedTimer = new Timer();
		speedTimer.WaitTime = changeDirectionTimer.WaitTime / 2.0;
		speedTimer.Timeout += Acceleration;
		AddChild(speedTimer);
		speedTimer.Start();
	}

	public void ChangeDirection() {
		velocity.X = (float)GD.RandRange(Min, Max);
		velocity.Y = (float)GD.RandRange(Min, Max);
		velocity.Z = (float)GD.RandRange(Min, Max);
		acceleration = 0;
	}

	public void Acceleration() {
		if (incrementalIncrease == 2)
			incrementalIncrease = -2;
		else
			incrementalIncrease = 2;
	}

	public override void _Process(double delta) {
		acceleration += incrementalIncrease * (float)0.1;
		moveableObject.Position += velocity * acceleration;
	}
}