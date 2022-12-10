using Godot;
using System;

public class Empty_tile : Tile
{

	Tween tween;

	
	Random random = new Random();
	float random_time;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		game_board = (Game_board)GetParent();

		//Random time for the rise animation
		random_time = random.Next(0, 11) * 0.1f * 0.4f + 0.2f;
		//Rise animation
		tween = (Tween)GetNode("Tween");
		tween.InterpolateProperty(this, "translation", new Vector3(this.Translation.x, this.Translation.y - 1.0f, this.Translation.z),
		new Vector3(this.Translation.x, this.Translation.y, this.Translation.z), random_time, Tween.TransitionType.Linear, Tween.EaseType.InOut);
		tween.Start();
	}

	//Function that gets called when a hex tile is clicked.
	private void _on_Area_input_event(object camera, object @event, Vector3 position, Vector3 normal, int shape_idx)
	{
		//Checks if it is a mouse button click
		if(@event is InputEventMouseButton inputEventMouseButton)
		{
			//Left mouse button click
			if(inputEventMouseButton.Pressed == true && inputEventMouseButton.ButtonIndex == 1)
			{
				game_board.click_empty_tile(this.xHex, this.zHex);
				game_board.check_finished_fields(this.xHex,this.zHex);
				QueueFree();
			}
			//Right mouse button click
			if(inputEventMouseButton.Pressed == true && inputEventMouseButton.ButtonIndex == 2)
			{
				GD.Print("Right mouse button click");
                game_board.occupiedPositions.Remove((this.xHex, this.zHex));
                QueueFree();
			}
		}
	}
}

