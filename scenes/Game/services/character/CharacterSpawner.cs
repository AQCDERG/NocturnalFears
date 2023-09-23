using Godot;
using GodotExtensions.scripts.utils;
using JayStation.prefabs._3d.character;

namespace JayStation.scenes.Game.services.character;

// TODO: When the godot devs fix their stuff, use TypedMultiplayerSpawner.

public partial class CharacterSpawner : MultiplayerSpawner {
	[Signal]
	public delegate void OnCharacterSpawnedEventHandler(Character character);

	Log log;

	[Export] protected PackedScene spawnableScene;

	public override void _EnterTree() {
		log = Log.From(GetScript());

		if (spawnableScene == null)
			log.Error("Spawnable scene is null, please set it in the inspector.");
		if (SpawnPath == null)
			log.Error("Spawn path is null, please set it in the inspector!");

		ClearSpawnableScenes();
		AddSpawnableScene(spawnableScene.ResourcePath);

		Spawned += node => {
			Character character = node as Character;
			EmitSignal(SignalName.OnCharacterSpawned, character);
		};
	}
}

// public partial class CharacterSpawner : TypedMultiplayerSpawner<CharacterSpawnInfo, Character> {
// 	protected override Character SpawnFunction_Callback(CharacterSpawnInfo spawnInfo, Character character) {
// 		int peerId = (int)spawnInfo.PeerId;
// 		character.PeerId = peerId;
// 		return character;
// 	}
// }