using Godot;
using System;
//THIS SCRIPT IS ATTACHED TO THE CAMERA
public partial class CameraController : Camera2D
{
	// Called when the node enters the scene tree for the first time.

	const float ZOOMFACTOR = .30f;
	const float ZOOMMIN = .1f;
	const float ZOOMMAX = 4f;
	public override void _Ready()
	{
		//Grab reference to the camera node in the scene tree
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsPhysicalKeyPressed(Key.W))
		{
			
			this.Position = new Vector2(this.Position.X, this.Position.Y - 1);
		}

		if (Input.IsPhysicalKeyPressed(Key.D))
		{
			this.Position = new Vector2(this.Position.X + 1, this.Position.Y);
			
		}
		if (Input.IsPhysicalKeyPressed(Key.S))
		{
			this.Position = new Vector2(this.Position.X, this.Position.Y + 1);
			
		}
		if (Input.IsPhysicalKeyPressed(Key.A))
		{
			
			this.Position = new Vector2(this.Position.X - 1, this.Position.Y);
		}
		zoom();
	}
	public void zoom()
	{

		if (Input.IsActionJustReleased("wheel_up"))
		{
			this.Zoom = new Vector2(Math.Clamp(this.Zoom.X + ZOOMFACTOR, ZOOMMIN, ZOOMMAX), 
			Math.Clamp(this.Zoom.Y + ZOOMFACTOR, ZOOMMIN, ZOOMMAX));
		}

		if (Input.IsActionJustReleased("wheel_down"))
		{
			this.Zoom = new Vector2(Math.Clamp(this.Zoom.X - ZOOMFACTOR, ZOOMMIN, ZOOMMAX), 
			Math.Clamp(this.Zoom.Y - ZOOMFACTOR, ZOOMMIN, ZOOMMAX));
		}
		
	}
}
