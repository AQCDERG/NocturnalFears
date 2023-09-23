using Godot;
using GodotExtensions.scripts.utils;

namespace JayStation.prefabs._3d.islands.managers;

public partial class Manager : Node
{
  protected Log log;

  public override void _EnterTree()
  {
    log = Log.From(GetScript());
  }
}