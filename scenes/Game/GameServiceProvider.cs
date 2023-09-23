using JayStation.scripts;

namespace JayStation.scenes.Game;

public partial class GameServiceProvider : ServiceProvider
{
  public override void _EnterTree()
  {
    base._EnterTree();
    GetParent<Game>().RegisterServiceProvider(this);
  }
}