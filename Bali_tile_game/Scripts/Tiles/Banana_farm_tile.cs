using Godot;
using System;

public class Banana_farm_tile : Tile
{


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		game_board = (Game_board)GetParent();
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//      
	//  }

	private void _on_Area_input_event(object camera, object @event, Vector3 position, Vector3 normal, int shape_idx)
	{
		//Checks if it is a mouse button click
		if (@event is InputEventMouseButton inputEventMouseButton)
		{
			//Left mouse button click
			if (inputEventMouseButton.Pressed == true && inputEventMouseButton.ButtonIndex == 1)
			{
				GD.Print("Left mouse button click");
			}
			//Right mouse button click
			if (inputEventMouseButton.Pressed == true && inputEventMouseButton.ButtonIndex == 2)
			{
				GD.Print("Right mouse button click");
				game_board.count_field_size(this.xHex, this.zHex);
			}
		}
	}
}
