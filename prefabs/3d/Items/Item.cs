using Godot;

namespace JayStation.prefabs._3d.Items;

public partial class Item : RigidBody3D {
	public TextureRect itemDisplay;
	public ClickDetectionComponenet clickDetectionComponenet;
	public bool hoveredOver;
	[Export] string name;


	public override void _Ready() {
		itemDisplay = GetNode<TextureRect>("Image");
		clickDetectionComponenet = GetNode<ClickDetectionComponenet>("ClickDetectionComponenet");
		clickDetectionComponenet.MouseEntered += onHover;
		clickDetectionComponenet.MouseExited += onHoverExit;
	}

	public void onHover() {
		hoveredOver = true;
		//clickDetectionComponenet.EmitSignal(SignalName.OnItemHovered, this);
	}

	public void onHoverExit() {
		hoveredOver = false;
	}

	public void onPicked(Slot itemSlot) {
		itemDisplay.Visible = true;
	}

	public void onActive() {
		GD.Print(name + "activate");
	}
}