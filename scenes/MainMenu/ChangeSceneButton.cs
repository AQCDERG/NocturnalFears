using Godot;

namespace JayStation.scenes.MainMenu;

public partial class ChangeSceneButton : Button {
	Node newScene;
	Node oldScene;
	[Export] string scenePath;
	[Export] string oldScenePath;

	public override void _Ready() {
		Pressed += changeScene;
		oldScene = GetTree().Root.GetNode<Node>(oldScenePath);
	}

	public void changeScene() {
		newScene = ResourceLoader.Load<PackedScene>(scenePath).Instantiate();
		GetTree().Root.AddChild(newScene);
		if (oldScene != null)
			GetTree().Root.RemoveChild(oldScene);
		else
			GD.Print("Old scene not found!");
		// MAKE IT SO IT REMOVES THE PREVIOUS SCENE
	}
}