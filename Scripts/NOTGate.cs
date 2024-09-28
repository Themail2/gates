using Godot;
using System;

public partial class NOTGate : BaseGate
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		dragDropBox = GetChild<TextureButton>(0);
		pinOneButton = GetChild<Button>(1);
		outputButton = GetChild<Button>(2);
		dragDropBox.ButtonDown += DragDropBoxPressed;
		pinOneButton.ButtonDown += InputPinOnePressed;
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
		outputState = !pinOneState;
    }
}