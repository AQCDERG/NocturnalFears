using System;
using Godot;
using GodotExtensions.extensions.node;
using GodotExtensions.scripts.onready;
using JayStation.components.other;

namespace JayStation.scripts.globals.autoloads;

public partial class GameState : Node {
	public static GameState Instance { get; private set; }
	[OnReady("Currency")] public Currency Currency { get; private set; }
	public Type TriggerdVillian { get; set; }

	public override void _EnterTree() {
		Instance = this;
	}

	public override void _Ready() {
		this.SetupNode();
	}
}