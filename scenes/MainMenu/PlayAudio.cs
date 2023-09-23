using Godot;

namespace JayStation.scenes.MainMenu;

public partial class PlayAudio : Node3D {
	AudioStreamPlayer audioStreamPlayer;
	[Export] float startFrom;

	public override void _Ready() {
		audioStreamPlayer = GetParent<AudioStreamPlayer>();
		audioStreamPlayer.Play(startFrom);
	}
}