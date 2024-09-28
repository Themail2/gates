using Godot;
using System;

public partial class NANDGate : BaseGate
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		dragDropBox = GetChild<TextureButton>(0);
		pinOneButton = GetChild<Button>(1);
		pinTwoButton = GetChild<Button>(2);
		outputButton = GetChild<Button>(3);
		dragDropBox.ButtonDown += DragDropBoxPressed;
		pinOneButton.ButtonDown += InputPinOnePressed;
		pinTwoButton.ButtonDown += InputPinTwoPressed;
		outputButton.ButtonDown += OutputPinPressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

    public void InputPinOnePressed()
	{
		if (GameManager.wireInHand == null)
		{
			if (pinOneWire == null)
				GameManager.PickUpWire(pinOneButton.GlobalPosition, this, PinType.pinOne);
			else
				GD.Print("Occupied");
		}
		else
		{
			if (pinOneWire == null)
				GameManager.PutDownWire(pinOneButton.GlobalPosition, this, PinType.pinOne);
			else
				GD.Print("Occupied");
		}
	}
	public void InputPinTwoPressed()
	{
		if (GameManager.wireInHand == null)
		{
			if (pinTwoWire == null)
				GameManager.PickUpWire(pinTwoButton.GlobalPosition, this, PinType.pinTwo);
			else
				GD.Print("Occupied");
		}
		else
		{
			if (pinTwoWire == null)
				GameManager.PutDownWire(pinTwoButton.GlobalPosition, this, PinType.pinTwo);
			else
				GD.Print("Occupied");
		}
	}

	public void OutputPinPressed()
	{
		if (GameManager.wireInHand == null)
			GameManager.PickUpWire(outputButton.GlobalPosition, this, PinType.output);
		else
			GameManager.PutDownWire(outputButton.GlobalPosition, this, PinType.output);
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

    public override void ComputeOutput()
    {
		outputState = !(pinOneState && pinTwoState);
    }
}