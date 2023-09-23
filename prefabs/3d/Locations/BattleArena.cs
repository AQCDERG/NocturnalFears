using Godot;

public partial class BattleArena : Node3D {
	public Node3D villianStartingLocation;
	public Node3D playerStaringLocation;

	public override void _Ready() {
		villianStartingLocation = GetNode<Node3D>("VillianStartingLocation");
		playerStaringLocation = GetNode<Node3D>("PlayerStartingLocation");
	}
}