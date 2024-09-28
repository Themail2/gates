using Godot;
using System;

public partial class Switch : BaseGate
{
	// Called when the node enters the scene tree for the first time.
	public CheckButton toggleSwitch;
	public override void _Ready()
	{
		toggleSwitch = GetChild<CheckButton>(2);
		outputButton = GetChild<Button>(3);
		dragDropBox = GetChild<TextureButton>(1);
		outputButton.ButtonDown += OutputPinPressed;
		toggleSwitch.Pressed += Toggled;
		dragDropBox.ButtonDown += DragDropBoxPressed;
	}
	public void OutputPinPressed()
	{
		if (GameManager.wireInHand == null)
			GameManager.PickUpWire(outputButton.GlobalPosition, this, PinType.output);
		else
			GameManager.PutDownWire(outputButton.GlobalPosition, this, PinType.output);
	}

	public void Toggled()
	{
		outputState = !outputState;
		GameManager.SimulateCircuit(this);
	}

	public void DragDropBoxPressed()
	{
		int i = 0;
		GameManager.PickUpGate(this);
		foreach(PinType pinType in outputPinList)
		{
			GD.Print("Pin ", i, " Type: ", pinType);
		}
		i = 0;
		GD.Print("This Gate ID: ", guid);
		foreach(BaseGate gate in outputs)
		{
			GD.Print("Gate ", i, " : " , gate.guid);
		}
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
