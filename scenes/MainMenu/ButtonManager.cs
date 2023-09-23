using Godot;

namespace JayStation.scenes.MainMenu; 

public partial class ButtonManager : Control {
	Button button;
	Node newScene;
	[Export] string scenePath;

	public override void _Ready() {
		button = GetParent<Button>();
		button.Pressed += changeScene;
	}

	public void changeScene() {
		newScene = ResourceLoader.Load<PackedScene>(scenePath).Instantiate();
		GetTree().Root.AddChild(newScene);
		// MAKE IT SO IT REMOVES THE PREVIOUS SCENE
	}
}