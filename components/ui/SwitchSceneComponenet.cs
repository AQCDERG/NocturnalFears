using Godot;

namespace JayStation.components.ui;

public partial class SwitchSceneComponenet : Node3D {
	[Export] string scenePath;

	public void ChangeScene() {
		GetTree().ChangeSceneToFile(scenePath);
	}
}