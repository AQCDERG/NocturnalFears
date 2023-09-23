using System.Linq;
using Godot;
using GodotExtensions.extensions.node;
using GodotExtensions.extensions.node.multiplayer_spawner;
using JayStation.levels;

namespace JayStation.scenes.Game.services.level;

public partial class LevelService : GameService
{
  [Export] public PackedScene LevelScene { get; set; }
  [Export] public Node3D LevelsHolder { get; set; }

  [Signal]
  public delegate void OnLevelAddedEventHandler(Level level);

  [Signal]
  public delegate void OnLevelRemovingEventHandler(Level level);

  MultiplayerSpawner levelSpawner;

  public override void _Ready()
  {

    LevelsHolder.ChildEnteredTree += node =>
    {
      if (node is Level level)
        EmitSignal(SignalName.OnLevelAdded, level);
      else
        log.Error($"Spawned node is not an level! {node.GetPath()}");
    };
    LevelsHolder.ChildExitingTree += node =>
    {
      if (node is Level level)
        EmitSignal(SignalName.OnLevelRemoving, level);
      else
        log.Error($"Despawned node is not an level! {node.GetPath()}");
    };

    Level level = LevelScene.Instantiate<Level>();
    AddLevel(level);
  }

  public void AddLevel(Level level)
  {
    LevelsHolder.AddChild(level);
  }
}