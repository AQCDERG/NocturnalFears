using Godot;
using GodotExtensions.scripts.utils;

namespace JayStation.scripts;

public partial class Service : Node
{

  protected ServiceProvider ServiceProvider { get; private set; }
  protected Log log;

  public override void _EnterTree()
  {
    log = Log.From(GetScript());
    ServiceProvider = GetParent<ServiceProvider>();
  }
}