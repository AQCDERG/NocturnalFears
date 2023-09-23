using Godot;
using GodotExtensions.scripts.utils;
using JayStation.components;

namespace JayStation.globals.components.Ammo;

[GlobalClass]
public partial class AmmoComponent : GlobalComponent {
	[Export] public int MaxAmmo { get; set; }
	[Export] public int CurrentAmmo { get; private set; }
	public bool IsEmpty => CurrentAmmo == 0;
	public bool IsFull => CurrentAmmo == MaxAmmo;
	Log log;


	public override void _EnterTree() {
		log = Log.From(GetScript());
		CurrentAmmo = MaxAmmo;
	}

	public void Refill() {
		CurrentAmmo = MaxAmmo;
	}

	public void AddAmmo(int amount) {
		// validate.
		if (amount == 0)
			log.Warn("AddAmmo called with amount 0");
		if (amount < 0)
			log.Error("AddAmmo called with negative amount");
		if (amount > MaxAmmo)
			log.Error("AddAmmo called with amount greater than MaxAmmo");

		CurrentAmmo = Mathf.Clamp(CurrentAmmo + amount, 0, MaxAmmo);
	}

	public bool UseAmmo(int amount = 1) {
		if (CurrentAmmo == 0)
			return false;
		if (amount < 0)
			log.Error("UseAmmo called with negative amount");
		if (amount == 0)
			log.Warn("UseAmmo called with amount 0");
		CurrentAmmo -= amount;
		if (CurrentAmmo < 0) {
			log.Error("CurrentAmmo is negative after UseAmmo");
			CurrentAmmo = 0;
		}

		return true;
	}

	public bool HasSufficientAmmo(int amount) {
		switch (amount) {
			case 0:
				log.Warn("HasSufficientAmmo called with amount 0");
				break;
			case < 0:
				log.Error("HasSufficientAmmo called with negative amount");
				return false;
		}

		return CurrentAmmo >= amount;
	}
}