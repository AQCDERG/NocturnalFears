using Godot;
using JayStation.prefabs._3d.Items;

namespace JayStation.components._3d;

public partial class DetectionComponent : Area3D {
	[Signal]
	public delegate void ValidBodyEnteredEventHandler(CharacterBody3D body);

	[Signal]
	public delegate void ValidBodyExitedEventHandler();

	[Signal]
	public delegate void ValidItemEnteredEventHandler(Item item);

	[Signal]
	public delegate void ValidItemExitedEventHandler();

	//public Optional<CharacterBody3D> CurrentTarget { get; private set; } = Optional<CharacterBody3D>.Nothing;
	public CharacterBody3D target;
	public Item itemInArea;

	public override void _Ready() {
		BodyEntered += CheckBodyType;
		BodyExited += CheckBodyTypeLeft;
	}

	public void CheckBodyType(Node3D body) {
		if (body is CharacterBody3D body3D && (body3D != Owner)) {
			target = body as CharacterBody3D;
			EmitSignal(SignalName.ValidBodyEntered, body);
		}

		if (body is Item item) {
			itemInArea = item;
			GD.Print(itemInArea.Name + " ENTERED");
			EmitSignal(SignalName.ValidItemEntered, item);
		}
	}

	public void CheckBodyTypeLeft(Node3D body) {
		if (body == target) {
			GD.Print("LEAVE " + target.Name);
			target = null;
			EmitSignal(SignalName.ValidBodyExited);
		}

		if (body == itemInArea) {
			GD.Print("LEAVE " + itemInArea.Name);

			itemInArea = null;

			EmitSignal(SignalName.ValidItemExited);
		}
	}
}