using Godot;
using JayStation.scripts.globals.autoloads;

namespace JayStation.components.ui;

public partial class HealthComponent : Node {
	[Export] int health = 100;
	[Export] Label healthLabel;

	public override void _Ready() {
		GameState.Instance.Currency.AddCurrency(100);
		healthLabel = GetNode<Label>("%HealthLabel");
		DisplayHealth();
	}

	public int GetHealth() => health;

	public void AddHealth(int amount) {
		health += amount;
	}

	public void TakeDamage(int amount) {
		health -= amount;
		DisplayHealth();
	}

	public void SetHealth(int amount) {
		health = amount;
	}

	public void DisplayHealth() {
		healthLabel.Text = GetHealth().ToString();
	}
}