using Godot;
using GodotExtensions.scripts.utils;
using JayStation.levels.managers;

namespace JayStation.levels;

public partial class Level : Node3D {
	public ManagerProvider Managers { get; private set; }
	MusicManager musicManager = new();
	AudioStreamPlayer music1;
	AudioStreamPlayer music2;
	AudioStreamPlayer music3;
	protected Log log;

	public override void _Ready() {
		music1 = GetNode<AudioStreamPlayer>("%Ambiant1");
		music2 = GetNode<AudioStreamPlayer>("%Ambiant2");
		music3 = GetNode<AudioStreamPlayer>("%Ambiant3");
		musicManager.AddMusic(music1);
		musicManager.AddMusic(music2);
		musicManager.AddMusic(music3);
		musicManager.SelectMusic();
	}

	public override void _EnterTree() {
		log = Log.From(GetScript());
	}

	public void RegisterManagerProvider(ManagerProvider provider) {
		if (Managers != null) {
			log.Error("ManagerProvider is already registered! " + GetPath());
			return;
		}

		Managers = provider;
	}
}