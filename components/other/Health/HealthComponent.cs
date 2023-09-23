using Godot;
using GodotExtensions.scripts.utils;
using JayStation.globals.components;

namespace JayStation.components._3d.Health;

// note, this will automatically go to the max health by default.

[GlobalClass]
public partial class HealthComponent : GlobalComponent {
	[Signal] // i dont like this name, but i dont know what else to call it.
	public delegate void OnDeathEventHandler();

	[Signal]
	public delegate void OnHealthChangedEventHandler(int newHealth);

	[Export] public int MaxHealth { get; set; } = 100;

	[Export]
	public int CurrentHealth {
		get => currentHealth;
		private set {
			if (currentHealth == value)
				return;
			currentHealth = value;
			ClampToMaxHealth();
			HandleDeath();
			EmitSignal(SignalName.OnHealthChanged, currentHealth);
		}
	}

	int currentHealth;
	Log log;

	void ClampToMaxHealth() {
		if (currentHealth > MaxHealth)
			currentHealth = MaxHealth;
	}

	void HandleDeath() {
		if (currentHealth > 0)
			return;
		currentHealth = 0;
		EmitSignal(SignalName.OnDeath);
	}

	public override void _EnterTree() {
		log = Log.From(GetScript());
		currentHealth = MaxHealth;
	}

	public void Remove(int damage) {
		if (damage < 0) {
			log.Error("Removal amount cannot be negative");
			return;
		}

		CurrentHealth -= damage;
	}

	public void Add(int amount) {
		if (amount < 0) {
			log.Error("Heal amount cannot be negative");
			return;
		}

		CurrentHealth += amount;
	}
}