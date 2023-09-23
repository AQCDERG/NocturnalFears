using System.Threading.Tasks;
using Godot;

namespace JayStation.scenes.Game.services.network;

public partial class NetworkService : GameService
{
  [Signal]
  public delegate void OnServerConnectedEventHandler();

  [Signal]
  public delegate void OnServerConnectionFailedEventHandler();

  [Signal]
  public delegate void OnServerDisconnectedEventHandler();

  const int SERVER_PEER_ID = 1;
  const string CERTIFICATE_FILE_PATH = "res://assets/ssl/game_server_certificate.crt";

  NodePath RootScenePath => Game.Instance.GetPath();


  public bool IsConnectedToServer => multiplayer.GetPeers().Length > 0;


  [Export] string ipAddress = "127.0.0.1";

  [Export] SceneMultiplayer multiplayer;

  [Export] ENetMultiplayerPeer network;

  [Export] int port = 4005;

  [Export] ENetPacketPeer serverPackerPeer;

  public override void _EnterTree()
  {
    base._EnterTree();

    return;
    InitializeNetwork();
    InitializeSceneMultiplayer();
    SetupNetworkCallbacks();

    OnServerConnected += () => { log.Info("Connected to the server."); };

    OnServerDisconnected += () =>
    {
      Client.Client.Instance.SwitchToLobby();
      log.Info("Disconnected from server going to lobby.");
    };

    OnServerConnectionFailed += () =>
    {
      Client.Client.Instance.SwitchToLobby();
      log.Info("Failed to connect to server going to lobby.");
    };
  }


  void InitializeNetwork()
  {
    network = new ENetMultiplayerPeer();
    network.RefuseNewConnections = true;
  }

  public void ConnectToServer()
  {
    log.Info($"Connecting to server at: \"{ipAddress}:{port}\"");
    Error error = network.CreateClient(ipAddress, port); // TODO: bandwith limits
    if (error != Error.Ok)
      log.Error("Failed to create server: ", error);

    // SetupEncryption();

    multiplayer.MultiplayerPeer = network;
  }

  void SetupEncryption()
  {
    network.Host.DtlsClientSetup("JayStation", GetTlsOptions());
  }

  TlsOptions GetTlsOptions()
  {
    X509Certificate cert = new();
    cert.Load(CERTIFICATE_FILE_PATH);
    return TlsOptions.ClientUnsafe(cert); // BEFORE-RELEASE: change to TlsOptions.Client()
  }

  void InitializeSceneMultiplayer()
  {
    multiplayer = new SceneMultiplayer();
    multiplayer.RootPath = RootScenePath;
    multiplayer.AllowObjectDecoding = false; // DO NOT ENABLE OR ELSE YOU WANT REMOTE CODE EXECUTION
                                             //_multiplayer.ServerRelay = false; // breaks everything if false.
    GetTree().SetMultiplayer(multiplayer, RootScenePath);
  }

  void SetupNetworkCallbacks()
  {
    multiplayer.ConnectedToServer += () => EmitSignal(SignalName.OnServerConnected);
    multiplayer.ServerDisconnected += () => EmitSignal(SignalName.OnServerDisconnected);

    //
    // _multiplayer.AuthCallback = new Callable(this, MethodName.MultiplayerAuthCallback);
    // _multiplayer.PeerAuthenticating += AuthenticatingWithServerCallback;
    // _multiplayer.PeerAuthenticationFailed += AuthenticatingWithServerFailedCallback;
  }

  void MultiplayerAuthCallback(long peerId, byte[] data)
  {
    // TODO: Implement actual authentication with backend.

    log.Info("Server authenticated.");
    Error error = multiplayer.CompleteAuth((int)peerId);
    if (error != Error.Ok)
      log.Info($"Server failed to complete auth?: {error}");
  }


  public async Task DisconnectFromServer()
  {
    const float GRACEFUL_DISCONNECT_DURATION_SECONDS = 3f;
    multiplayer.DisconnectPeer(SERVER_PEER_ID);
    await ToSignal(GetTree().CreateTimer(GRACEFUL_DISCONNECT_DURATION_SECONDS), SceneTreeTimer.SignalName.Timeout);
    if (serverPackerPeer.IsActive())
      serverPackerPeer.PeerDisconnectNow();
  }
}