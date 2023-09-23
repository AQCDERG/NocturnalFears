using Godot;

namespace JayStation.scenes.MainMenu;

public partial class TeleportPad : CsgCylinder3D {
	Area3D area3D;

	public override void _Ready() {
		area3D = GetNode<Area3D>("Area3D");

		area3D.BodyEntered += body => {
			if (body.Name != "CharacterController") return;
			Client.Client.Instance.SwitchToWorld();
		};
	}
}