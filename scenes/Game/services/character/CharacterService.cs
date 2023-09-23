using System.Threading.Tasks;
using Godot;
using JayStation.prefabs._3d.character;

namespace JayStation.scenes.Game.services.character;

public partial class CharacterService : GameService {
	[Signal]
	public delegate void OnLocalCharacterSpawnedEventHandler(Character character);

	public Character LocalCharacter { get; private set; }
	public bool HasLocalCharacter => LocalCharacter != null;

	CharacterSpawner characterSpawner;

	public Task<Character> WaitForCharacter() {
		if (HasLocalCharacter)
			return Task.FromResult(LocalCharacter);

		TaskCompletionSource<Character> tcs = new();

		characterSpawner.OnCharacterSpawned += Handler;
		return tcs.Task;

		void Handler(Character character) {
			tcs.SetResult(character);
			characterSpawner.OnCharacterSpawned -= Handler;
		}
	}

	public override void _Ready() {
		// characterSpawner = GetNode<CharacterSpawner>("CharacterSpawner");
		// characterSpawner.OnCharacterSpawned += character => {
		// 	if (character.IsLocal) {
		// 		log.Info("Local character spawned.");
		// 		LocalCharacter = character;
		// 		EmitSignal(SignalName.OnLocalCharacterSpawned, character);
		// 	}
		// };
	}
}