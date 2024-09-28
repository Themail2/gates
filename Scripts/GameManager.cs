using Godot;
using Godot.NativeInterop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using System.Runtime.CompilerServices;
public enum PinType
{
	pinOne,
	pinTwo,
	output
}
public partial class GameManager : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public static PackedScene NAND = GD.Load<PackedScene>("res://Scenes/NAND.tscn");

	public static PackedScene WIRE = GD.Load<PackedScene>("res://Scenes/Wire.tscn");
	public static PackedScene HUD = GD.Load<PackedScene>("res://Scenes/HUD.tscn");
	public static PackedScene LIGHT = GD.Load<PackedScene>("res://Scenes/Light.tscn");
	public static PackedScene SWITCH = GD.Load<PackedScene>("res://Scenes/Switch.tscn");
	public static BaseGate gateInHand = null;
	public static List<BaseGate> allGates = new List<BaseGate>();
	public static Wire wireInHand = null;
	public static Node baseNode;
	public static Vector2 mousePos;

	public static Vector2 OFFSET = new Vector2(15,15);
	public override void _Ready()
	{
		baseNode = GetNode(".");
		baseNode.AddChild(HUD.Instantiate<HUDScript>());
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		mousePos = GetGlobalMousePosition();
		GateInHandProcessing(mousePos);
		WireInHandProcessing(mousePos);
	}

	public static void SimulateCircuit(BaseGate root)
	{
		Queue<BaseGate> queue = new Queue<BaseGate>();
		BaseGate currentNode;
		BaseGate childNode;
		//Queue up the root node
		queue.Enqueue(root);
		while(queue.Count != 0)
		{
			//DeQueue the currentNode
			currentNode = queue.Dequeue();
			//Compute its output
			currentNode.ComputeOutput();
			//Increment the visit count
			currentNode.visits += 1;
			//Set its output to the inputs of its children
			for(int i = 0; i < currentNode.outputs.Count; i++)
			{
				childNode = currentNode.outputs[i];
				switch(currentNode.outputPinList[i])
				{
					case PinType.pinOne:
						childNode.pinOneState = currentNode.outputState;
						break;
					case PinType.pinTwo:
						childNode.pinTwoState = currentNode.outputState;
						break;
				}
				// Queue up the children of the currentNode if they've been visited less than 3 times
				if(childNode.visits < 3)
					queue.Enqueue(childNode);
			}
		}
		foreach(BaseGate gate in allGates)
		{
			gate.visits = 0;
		}
	}
	public static void PickUpWire(Vector2 pos, BaseGate gate, PinType pinType)
	{
		wireInHand = WIRE.Instantiate<Wire>();
		wireInHand.AddPoint(pos + OFFSET);
		wireInHand.AddPoint(pos + OFFSET);

		switch(pinType)
		{
			case PinType.pinOne:
				gate.pinOneWire = wireInHand;
				wireInHand.inputGate = gate;
				wireInHand.inputType = PinType.pinOne;
				break;

			case PinType.pinTwo:
				gate.pinTwoWire = wireInHand;
				wireInHand.inputGate = gate;
				wireInHand.inputType = PinType.pinTwo;
				break;

			case PinType.output:
				gate.outputWires.Add(wireInHand);
				wireInHand.outputGate = gate;
				wireInHand.outputType = PinType.output;
				break;
		}

		baseNode.AddChild(wireInHand);
	}

	public static void PutDownWire(Vector2 pos, BaseGate gate, PinType pinType)
	{
		if (wireInHand.inputGate == gate || wireInHand.outputGate == gate)
		{
			GD.Print("Gates cannot connect to themselves");
			return;
		}

		switch(pinType)
		{
			case PinType.pinOne:
				if (wireInHand.inputGate != null)
				{
					GD.Print("Inputs must connect to outputs");
					return;
				}
				gate.pinOneWire = wireInHand;
				wireInHand.inputGate = gate;
				wireInHand.inputType = PinType.pinOne;
				break;

			case PinType.pinTwo:
				if (wireInHand.inputGate != null)
				{
					GD.Print("Inputs must connect to outputs");
					return;
				}
				gate.pinTwoWire = wireInHand;
				wireInHand.inputGate = gate;
				wireInHand.inputType = PinType.pinTwo;
				break;
				
			case PinType.output:
				if (wireInHand.outputGate != null)
				{
					GD.Print("Inputs must connect to outputs");
					return;
				}
				gate.outputWires.Add(wireInHand);
				wireInHand.outputGate = gate;
				wireInHand.outputType = PinType.output;
				break;
		}
		/* 
		Connect the nodes 🎉
		so, the wire has an input gate, which means its attached to an input pin
		and an output gate, which means its attached to an output pin.
		That means that the output gate is connected to the input gate.
		So, we can add the input gate as a child of the output gate to create a connection
		*/
		wireInHand.outputGate.outputs.Add(wireInHand.inputGate);
		// Make sure the relevent parrallel data is there 😎
		wireInHand.outputGate.outputPinList.Add(wireInHand.inputType);
		// CHUCK THAT SHIT OUT 😡😡😡😡😡😡😡😡😡😡😡😡
		wireInHand = null;
	}
	public static void WireInHandProcessing(Vector2 mousePos)
	{
		if(wireInHand != null)
		{
			if(Input.IsPhysicalKeyPressed(Key.Escape))
			{
				if(wireInHand.inputGate != null)
				{
					if(wireInHand.inputGate.pinOneWire == wireInHand)
						wireInHand.inputGate.pinOneWire = null;
					if(wireInHand.inputGate.pinTwoWire == wireInHand)
						wireInHand.inputGate.pinTwoWire = null;
					wireInHand.inputGate.outputWires.Remove(wireInHand);
				}
				if(wireInHand.outputGate != null)
				{
					if(wireInHand.outputGate.pinOneWire == wireInHand)
						wireInHand.outputGate.pinOneWire = null;
					if(wireInHand.outputGate.pinTwoWire == wireInHand)
						wireInHand.outputGate.pinTwoWire = null;
					wireInHand.outputGate.outputWires.Remove(wireInHand);
				}
				wireInHand.Free();
				wireInHand = null;
				return;
			}

			if (wireInHand.inputGate != null)
			{
				wireInHand.Points = new Vector2[] { new Vector2(wireInHand.Points[0].X, wireInHand.Points[0].Y),
				new Vector2(mousePos.X , mousePos.Y)  };
			}
			else
			{
				wireInHand.Points = new Vector2[] { new Vector2(mousePos.X , mousePos.Y) ,
				new Vector2(wireInHand.Points[1].X, wireInHand.Points[1].Y)};
			}
		}
	}
	public static void GateInHandProcessing(Vector2 mousePos)
	{
		if(gateInHand != null)
		{
			gateInHand.Position = mousePos;
			if(Input.IsActionJustReleased("left_mouse"))
			{
				gateInHand = null;
				return;
			}
			
			if(gateInHand.pinOneWire != null)
			{
				gateInHand.pinOneWire.Points = new Vector2[] { new Vector2(gateInHand.pinOneButton.GlobalPosition.X , gateInHand.pinOneButton.GlobalPosition.Y) + OFFSET,
				new Vector2(gateInHand.pinOneWire.Points[1].X, gateInHand.pinOneWire.Points[1].Y)};
			}

			if(gateInHand.pinTwoWire != null)
			{
				gateInHand.pinTwoWire.Points = new Vector2[] { new Vector2(gateInHand.pinTwoButton.GlobalPosition.X , gateInHand.pinTwoButton.GlobalPosition.Y) + OFFSET,
				new Vector2(gateInHand.pinTwoWire.Points[1].X, gateInHand.pinTwoWire.Points[1].Y)};
			}

			if(gateInHand.outputWires.Count > 0)
			{
				foreach (Wire outputWire in gateInHand.outputWires)
				{
					outputWire.Points = new Vector2[] { new Vector2(outputWire.Points[0].X, outputWire.Points[0].Y),
				new Vector2(gateInHand.outputButton.GlobalPosition.X , gateInHand.outputButton.GlobalPosition.Y) + OFFSET };
				}
			}
		}
	}
	public static void PickUpGate(BaseGate gate)
	{
		gateInHand = gate;
	}

	public static void PickUpNAND(Vector2 pos)
	{
		gateInHand = NAND.Instantiate<BaseGate>();
		baseNode.AddChild(gateInHand);
		allGates.Add(gateInHand);
	}

	public static void PickUpSwitch(Vector2 pos)
	{
		gateInHand = SWITCH.Instantiate<BaseGate>();
		baseNode.AddChild(gateInHand);
		allGates.Add(gateInHand);
	}

	public static void PickUpLight(Vector2 pos)
	{
		gateInHand = LIGHT.Instantiate<BaseGate>();
		baseNode.AddChild(gateInHand);
		allGates.Add(gateInHand);
	}
}
