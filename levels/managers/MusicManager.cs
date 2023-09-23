using System;
using System.Collections.Generic;
using Godot;

namespace JayStation.levels.managers;

public partial class MusicManager : Node {
	public List<AudioStreamPlayer> ambiantMusicList = new();

	public void SelectMusic() {
		Random random = new();
		int randomMusic = random.Next(0, ambiantMusicList.Count);
		ambiantMusicList[randomMusic].Play();
	}

	public void AddMusic(AudioStreamPlayer music) {
		ambiantMusicList.Add(music);
	}
}