using JayStation.scripts;

namespace JayStation.scenes.Game.services;

public abstract partial class GameService : Service
{
  public override void _EnterTree()
  {
    base._EnterTree();
    // Game.Instance.RegisterService(this);
  }
}