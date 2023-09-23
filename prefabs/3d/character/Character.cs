using Godot;
using JayStation.components._3d;
using JayStation.components.ui;
using JayStation.prefabs._3d.Items;

namespace JayStation.prefabs._3d.character;

public partial class Character : CharacterBody3D {
	const float SPEED = 2f;
	const float ACCELERATION = 0.5f;

	public bool IsLocal { get; private set; } = true;
	public HealthComponent health;
	Label healthLabel;
	int headSpeed = 5;
	float jumpPower = 5.0f;
	Camera3D camera;
	Node3D head;
	InventoryComponenet inventory;
	Vector3 velocity;
	DetectionComponent detectionComponent;
	Item itemInArea;
	RichTextLabel displayName;

	public override void _EnterTree() {
		head = GetNode<Node3D>("%Head");
		camera = GetNode<Camera3D>("%playerCamera");
	}

	public override void _Ready() {
		health = GetNode<HealthComponent>("%HealthComponent");
		healthLabel = GetNode<Label>("%HealthLabel");
		health.SetHealth(100);
		inventory = GetNode<InventoryComponenet>("%Inventory");
		detectionComponent = GetNode<DetectionComponent>("%DetectionComponent");
		detectionComponent.ValidItemEntered += itemEntered;
		detectionComponent.ValidItemExited += itemExited;
		displayName = GetNode<RichTextLabel>("%ItemLabel");
	}

	public override void _PhysicsProcess(double delta) {
		Vector2 inputDir =
			Input.GetVector("movement_left", "movement_right", "movement_forward", "movement_backward");
		Vector3 direction = head.GlobalTransform.Basis * new Vector3(inputDir.X * headSpeed, 0, inputDir.Y * headSpeed);
		//implement delta into movement
		if (direction != Vector3.Zero) {
			velocity.X = direction.X * SPEED;
			velocity.Z = direction.Z * SPEED;
		}
		else {
			velocity.X = Mathf.MoveToward(Velocity.X, 0, ACCELERATION);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, ACCELERATION);
		}

		if (Input.IsActionPressed("movement_jump") && IsOnFloor()) velocity.Y = jumpPower;

		// Gravity (considering frame rate independence)
		velocity.Y -= 5.8f * (float)delta;

		Velocity = velocity;
		MoveAndSlide();
	}

	public override void _Input(InputEvent @event) {
		//Input.MouseMode = Input.MouseModeEnum.Captured;
		if (@event is InputEventMouseMotion mouseMotion) {
			head.RotateY(-mouseMotion.Relative.X * 0.01f);
			camera.RotateX(-mouseMotion.Relative.Y * 0.01f);
			Vector3 cameraRot = camera.Rotation;
			cameraRot.X = Mathf.Clamp(cameraRot.X, Mathf.DegToRad(-80f), Mathf.DegToRad(80f));
			camera.Rotation = cameraRot;
		}

		if (itemInArea != null)
			if (itemInArea.hoveredOver) {
				displayName.Text = "Press E to pick up " + itemInArea.Name;
				if (Input.IsActionJustPressed("pick_up")) {
					inventory.addItem(detectionComponent.itemInArea);
					GD.Print("NO DYLAN NO");
				}
			}

		if (Input.IsActionPressed("movement_sprint")) headSpeed = 10;
		if (Input.IsActionJustReleased("movement_sprint")) headSpeed = 5;
	}

	public void itemEntered(Item item) {
		itemInArea = item;
	}

	public void itemExited() {
		itemInArea = null;
		displayName.Text = "";
	}
}