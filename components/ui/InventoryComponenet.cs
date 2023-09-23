using System.Collections.Generic;
using System.Linq;
using Godot;
using GodotExtensions.extensions.node;
using JayStation.prefabs._3d.Items;

public partial class InventoryComponenet : Node3D {
	List<Item> items;
	List<ItemSlot> Slots;
	TextureRect InventoryDisplay;

	public override void _Ready() {
		Slots = this.GetDescendantsWhichAreA<ItemSlot>().ToList();
		InventoryDisplay = GetNode<TextureRect>("%VisualInventory");
		items = new List<Item>();
		//for (int i = 0; i < 7; i++) listOfSlots[i] = itemslot1 = GetNode<ItemSlot>("ItemSlot" + i + 1);
	}

	public void addItem(Item item) {
		items.Add(item);
		displayItem();
	}

	public void displayItem() {
		int itteration = 0;
		GD.Print(Slots.Capacity);
		foreach (Item item in items) {
			Slots[itteration].itemInSlot = item;
			item.itemDisplay.Position =
				Slots[itteration].Position * new Vector2(0.25f, 0.25f) +
				InventoryDisplay.Position; // IF YOU ARE CRYING ABOUT 0.25 IT IS BECAUSE OF SCALING OK CRY ABOUT IT
			item.itemDisplay.Visible = true;
			itteration++;
		}
	}

	public override void _Input(InputEvent @event) {
		if (Input.IsActionPressed("slot1")) Slots[0].activate();
	}
}