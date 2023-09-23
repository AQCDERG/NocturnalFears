using Godot;
using GodotExtensions.scripts.utils;
using JayStation.scenes.Game.services;
using JayStation.scenes.Game.services.network;
using JayStation.scripts;

namespace JayStation.scenes.Game;

public partial class Game : Node
{
  [Signal]
  public delegate void OnServiceRegisteredEventHandler(GameService service);

  public static Game Instance { get; private set; }
  public ServiceProvider Services { get; private set; }

  Log log;

  public override void _EnterTree()
  {
    Instance = this;
    log = Log.From(GetScript());
  }

  public void RegisterServiceProvider(ServiceProvider provider)
  {
    if (Services != null)
    {
      log.Error("ServiceProvider is already registered!");
      return;
    }

    Services = provider;
  }

  public override void _Ready()
  {
    // Services.GetService<NetworkService>().ConnectToServer();
  }

  public void Shutdown()
  {
    // todo, add shutdown reason?
    _ = Services.WaitForService<NetworkService>().ContinueWith(async task =>
    {
      NetworkService networkService = await task;
      _ = networkService.DisconnectFromServer();
    });
    Client.Client.Instance.SwitchToLobby();
  }
}