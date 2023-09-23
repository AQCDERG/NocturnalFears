using Godot;

namespace JayStation.scenes.Client;

public partial class Client : Node {
	public static Client Instance { get; private set; }

	[Export] public PackedScene LobbyScene { get; set; }
	[Export] public PackedScene GameScene { get; set; }
	Node gameHolder;
	Node lobbyHolder;

	public override void _EnterTree() {
		Instance = this;
	}

	public override void _Ready() {
		lobbyHolder = GetNode<Node>("%Lobby Holder");
		gameHolder = GetNode<Node>("%Game Holder");
		SwitchToLobby();
	}

	public void SwitchToLobby() {
		UnloadWorld();
		LoadLobby();
	}

	void UnloadWorld() {
		Node possibleWorld = gameHolder.GetChildOrNull<Node>(0);
		possibleWorld?.QueueFree();
	}

	void LoadLobby() {
		Node newLobby = LobbyScene.Instantiate();
		lobbyHolder.AddChild(newLobby);
	}

	public void SwitchToWorld() {
		UnloadLobby();
		LoadGame();
	}

	void UnloadLobby() {
		Node possibleLobby = lobbyHolder.GetChildOrNull<Node>(0);
		possibleLobby?.QueueFree();
	}

	void LoadGame() {
		Node newGame = GameScene.Instantiate();
		gameHolder.AddChild(newGame);
	}
}