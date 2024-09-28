using Godot;
using System;
using System.Collections.Generic;

public partial class BaseGate : Node2D
{

	public Button pinOneButton, pinTwoButton, outputButton;
	public TextureButton dragDropBox;
	public Line2D pinOneWire, pinTwoWire = null;
	public List<Line2D> outputWires = new List<Line2D>();
	//				^ Dumb Godot Stuff ^
	//________--------```````````````--------__________//
	public bool pinOneState, pinTwoState, outputState = false;
	public List<BaseGate> outputs = new List<BaseGate>();
	//Parrallel list, parrallel to the outputs list of BaseGate, lets us know where to set the output
	public List<PinType> outputPinList = new List<PinType>();
	// We need to know if our "pins" are occupied
	public short visits = 0;
	//________--------```````````````--------__________//
	public Guid guid = Guid.NewGuid();
	public override void _Ready()
	{
		
	}
	
	public override void _Process(double delta)
	{

	}
	public virtual void ComputeOutput()
	{

	}
}	