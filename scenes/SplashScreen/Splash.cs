using System.Threading.Tasks;
using Godot;
using Godot.Collections;
using GodotExtensions.scripts.utils;

namespace JayStation.scenes.Splash;

public partial class Splash : Control {
	const bool USE_SUB_THREADS = false;

	readonly Array loadStatus = new();

	[Export(PropertyHint.File)] string gamePackedScenePath;
	bool loading = true;
	Log log;
	ProgressBar progressBar;

	public override void _EnterTree() {
		log = Log.From(GetScript());
	}

	public override void _Ready() {
		progressBar = GetNode<ProgressBar>("Progress");
		log.Info("Loading game.");
		LoadGame();
	}

	public override void _Process(double delta) {
		if (!loading)
			return;
		int loadingProgress = PollGameLoadStatus();
		progressBar.Value = loadingProgress;

		if (loadingProgress == 100) {
			loading = false;
			log.Info("Starting game.");
			_ = StartGame();
		}
	}

	void LoadGame() {
		Error error = ResourceLoader.LoadThreadedRequest(gamePackedScenePath, "PackedScene");
		if (error != Error.Ok)
			log.Error("Failed to load game scene: ", error);
	}

	// Returns the persentage between 0 and 100.
	int PollGameLoadStatus() {
		ResourceLoader.ThreadLoadStatus loadStatus =
			ResourceLoader.LoadThreadedGetStatus(gamePackedScenePath, this.loadStatus);

		switch (loadStatus) {
			case ResourceLoader.ThreadLoadStatus.Failed:
				log.Error("Failed to load game scene: ", loadStatus);
				loading = false;
				break;

			case ResourceLoader.ThreadLoadStatus.InvalidResource:
				log.Info("Invalid resource: ", loadStatus);
				loading = false;
				break;

			case ResourceLoader.ThreadLoadStatus.InProgress:
				return (int)this.loadStatus[0];

			case ResourceLoader.ThreadLoadStatus.Loaded:
				return 100;
		}

		return 0;
	}

	async Task StartGame() {
		// might still be setting up current scene, so just wait for it to finish.
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		PackedScene gameScenePacked = GetGameScene();
		GetTree().ChangeSceneToPacked(gameScenePacked);
	}

	PackedScene GetGameScene() => ResourceLoader.LoadThreadedGet(gamePackedScenePath) as PackedScene;
}