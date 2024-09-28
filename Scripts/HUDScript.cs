using Godot;
using System;

public partial class HUDScript : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void GateSelected(int index, Vector2 pos, int mouseButton)
	{
		GD.Print(index);
		switch(index)
		{
			case 0:
				GameManager.PickUpNAND(pos);
				GD.Print("NAND");
				break;
			case 1:
				GameManager.PickUpSwitch(pos);
				GD.Print("Switch");
				break;
			case 6:
				GameManager.PickUpLight(pos);
				break;
		}
	}
}
