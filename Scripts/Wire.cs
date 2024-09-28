using Godot;
using System;

public partial class Wire : Line2D
{
	// Called when the node enters the scene tree for the first time.

	
	public BaseGate inputGate, outputGate = null;

	public PinType inputType, outputType;

	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(outputGate.outputState)
		{
			DefaultColor = new Color("GREEN");
			return;
		}
		DefaultColor = new Color("RED");
	}
}
