using Godot;
using JayStation.prefabs._3d.Items;

public partial class ItemSlot : TextureRect {
	public Item itemInSlot;

	public void displayItem() {
		itemInSlot.itemDisplay.Visible = true;
	}

	public void activate() {
		GD.Print("activate");
	}
}