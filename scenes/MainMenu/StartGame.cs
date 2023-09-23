using Godot;

namespace JayStation.scenes.MainMenu;

public partial class StartGame : Button {
	AudioStreamPlayer jayStationAudio;
	TextureRect menu;

	public override void _Ready() {
		jayStationAudio = GetNode<AudioStreamPlayer>("%LetsGetIt");
		menu = GetNode<TextureRect>("%ChooseLevel");
		Pressed += ChangeToMainScene;
	}

	void ChangeToMainScene() {
		//Client.Client.Instance.SwitchToWorld();
		jayStationAudio.Play();
		menu.Visible = true;
	}
}