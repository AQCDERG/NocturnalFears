using Godot;

namespace JayStation.globals.components.Reload;

[GlobalClass]
public partial class IncrementalReloadComponent : ReloadComponent
{
  [Export] public int AmmoPerReloadIncrement { get; set; } = 1;

  protected override void HandleReloadIncrement()
  {
    int ammoToBeLoaded = GetAmmoToBeLoaded();

    AmmoComponent.AddAmmo(ammoToBeLoaded);
    EmitSignal(ReloadComponent.SignalName.OnAmmoLoaded, ammoToBeLoaded);

    if (AmmoComponent.IsFull)
      FinishReload();
    else // reload again.
      HandleReloadStart();
  }

  protected override void HandleReloadFinished()
  {
    // In the base class, the ammo is fully reloaded here, but we don't need it.
  }

  protected override void ConfigureTimer()
  {
    double timePerIncrement = TotalReloadTimeSeconds / GetAmmoToBeLoaded();
    timer.WaitTime = timePerIncrement;
  }

  int GetAmmoToBeLoaded()
  {
    int ammoTillFull = AmmoComponent.MaxAmmo - AmmoComponent.CurrentAmmo;
    return Mathf.Min(ammoTillFull, AmmoPerReloadIncrement);
  }
}