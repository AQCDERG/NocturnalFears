using System;
using Godot;
using GodotExtensions.extensions.node;
using JayStation.prefabs._3d.Enemies.BattleEnemies;
using JayStation.prefabs._3d.Enemies.MrTickleClown;
using JayStation.scenes.Characters;
using JayStation.scripts.globals.autoloads;

namespace JayStation.scenes.BattleScene;

public partial class BattleSceneManager : Node3D {
	[Export] public PackedScene battleSawScene;
	[Export] public PackedScene wareHouseScene;
	[Export] public PackedScene battleClownScene;
	Control startingEffect;
	public PackedScene player = ResourceLoader.Load("res://prefabs/3d/character/Character.tscn") as PackedScene;
	Timer timer;

	public override void _Ready() {
		Type triggeredVillian = GameState.Instance.TriggerdVillian;
		startingEffect = GetNode<Control>("%StartEffects");
		BattleVillian battleVillian = null;
		BattleArena battleArena = null;

		if (triggeredVillian == typeof(JigSaw)) {
			battleVillian = battleSawScene.Instantiate<BattleVillian>();
			battleArena = wareHouseScene.Instantiate<BattleArena>();
		}
		else if (triggeredVillian == typeof(MrTickleClown)) {
			battleVillian = battleClownScene.Instantiate<BattleVillian>();
			battleArena = wareHouseScene.Instantiate<BattleArena>();
		}
		else
			throw new Exception("No villian found");

		AddChild(battleVillian);
		AddChild(battleArena);
		StartBattle(battleVillian, battleArena);
	}


	public async void StartBattle(BattleVillian battleVillian, BattleArena battleArena) {
		await this.YieldForSeconds(3);
		createPlayer(battleArena);
		battleVillian.themeSong.Play();
		battleVillian.Position = battleArena.villianStartingLocation.Position;
		battleVillian.SetProcess(true);
		startingEffect.QueueFree();
	}

	public void createPlayer(BattleArena battleArena) {
		CharacterBody3D Player = player.Instantiate<CharacterBody3D>();
		AddChild(Player);
		Player.Position = battleArena.playerStaringLocation.Position;
	}
}