using Godot;
using JayStation.components;

namespace JayStation.globals.components.Damage;

[GlobalClass]
public partial class DamageComponent : GlobalComponent {
	[Export] public int Amount { get; set; }

	// later on this can be used to calculate damage based on distance from the source
	// in reality its a node3d.
	public virtual int GetDamageTo(object obj) => Amount;
}