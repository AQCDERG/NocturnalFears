using Godot;
using JayStation.components;
using JayStation.globals.components.Ammo;

namespace JayStation.globals.components.Reload;

[GlobalClass]
public partial class ReloadComponent : GlobalComponent {
	[Signal] // Leftovers don't matter because you figure out how much ammo is used from here.
	public delegate void OnAmmoLoadedEventHandler(int amountLoaded);

	[Signal]
	public delegate void OnReloadCancelledEventHandler();

	[Signal]
	public delegate void OnReloadFinishedEventHandler();

	[Signal]
	public delegate void OnReloadStartedEventHandler();

	[Export] public double TotalReloadTimeSeconds { get; set; }
	[Export] public AmmoComponent AmmoComponent { get; set; }
	[Export] public bool IsReloading { get; protected set; }

	protected Callable handleReloadIncrement;
	protected Timer timer;

	public override void _EnterTree() {
		CreateTimer();
		handleReloadIncrement = new Callable(this, MethodName.HandleReloadIncrement);
	}

	void CreateTimer() {
		timer = new Timer();
		timer.OneShot = true;
		AddChild(timer);
	}


	public void CancelReloading() {
		if (!IsReloading) // If we're not reloading, we don't need to stop?
			return;

		IsReloading = false;

		timer.Stop();
		EmitSignal(SignalName.OnReloadCancelled);
	}

	public void StartReloading() {
		if (IsReloading)
			return;
		if (AmmoComponent.IsFull)
			return;

		IsReloading = true;

		EmitSignal(SignalName.OnReloadStarted);
		HandleReloadStart();
	}

	protected virtual void HandleReloadStart() {
		ReconnectTimerCallbacks();
		RefreshTimer();
	}

	void ReconnectTimerCallbacks() {
		// may be unnecessary
		if (timer.IsConnected(Timer.SignalName.Timeout, handleReloadIncrement))
			timer.Disconnect(Timer.SignalName.Timeout, handleReloadIncrement);
		timer.Connect(Timer.SignalName.Timeout, handleReloadIncrement);
	}

	void RefreshTimer() {
		ConfigureTimer();
		timer.Stop();
		timer.Start();
	}

	protected virtual void ConfigureTimer() {
		timer.WaitTime = TotalReloadTimeSeconds;
	}

	protected virtual void HandleReloadIncrement() {
		FinishReload();
	}

	protected void FinishReload() {
		HandleReloadFinished();
		IsReloading = false;
		EmitSignal(SignalName.OnReloadFinished);
	}

	protected virtual void HandleReloadFinished() {
		int ammoUsed = AmmoComponent.MaxAmmo - AmmoComponent.CurrentAmmo;
		AmmoComponent.Refill(); // could be problematic by giving the player free ammo.
		EmitSignal(SignalName.OnAmmoLoaded, ammoUsed);
	}
}