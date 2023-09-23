using Godot;

namespace JayStation.components.other;

[GlobalClass]
public partial class Currency : GlobalComponent {
	public int Amount { get; private set; }

	public void AddCurrency(int amount) {
		Amount += amount;
	}

	public void RemoveCurrency(int amount) {
		Amount -= amount;
	}

	public void SetCurrency(int amount) {
		Amount = amount;
	}
}