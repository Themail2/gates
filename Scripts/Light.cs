using Godot;
using System;

public partial class Light : BaseGate
{
	// Called when the node enters the scene tree for the first time.
	ColorRect rect;
	public override void _Ready()
	{
		pinOneButton = GetChild<Button>(2);
		dragDropBox = GetChild<TextureButton>(1);
		rect = GetChild<ColorRect>(3);
		dragDropBox.ButtonDown += DragDropBoxPressed;
		pinOneButton.ButtonDown += InputPinOnePressed;
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
		if(pinOneState)
		{
			rect.Color = new Color("YELLOW");
			return;
		}
		rect.Color = new Color("GRAY");
	}
}
