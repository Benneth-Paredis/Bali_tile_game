using Godot;
using System;

public class Empty_tile : Tile
{
    private Game_board game_board;

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

	//Function that gets called when a hex tile is clicked.
	private void _on_Area_input_event(object camera, object @event, Vector3 position, Vector3 normal, int shape_idx)
	{
		//Checks if it is a mouse button click
		if(@event is InputEventMouseButton inputEventMouseButton)
		{
			//Left mouse button click
			if(inputEventMouseButton.Pressed == true && inputEventMouseButton.ButtonIndex == 1)
			{
				GD.Print("Left Mouse button click");
				spawnTile();
			}
			//Right mouse button click
			if(inputEventMouseButton.Pressed == true && inputEventMouseButton.ButtonIndex == 2)
			{
				GD.Print("Right mouse button click");
				this.Visible = false;
			}
		}
	}

	//Spawn 6 empty tiles surrounding the clicked tile and deleting it.
	private void spawnTile()
	{
		game_board.spawnTile("empty_tile", this.Translation.x + 2*game_board.tileApothem, this.Translation.z);
		game_board.spawnTile("empty_tile", this.Translation.x - 2*game_board.tileApothem, this.Translation.z);
		game_board.spawnTile("empty_tile", this.Translation.x + game_board.tileApothem, this.Translation.z + 1.5f);
		game_board.spawnTile("empty_tile", this.Translation.x + game_board.tileApothem, this.Translation.z - 1.5f);
		game_board.spawnTile("empty_tile", this.Translation.x - game_board.tileApothem, this.Translation.z + 1.5f);
		game_board.spawnTile("empty_tile", this.Translation.x - game_board.tileApothem, this.Translation.z - 1.5f);

		QueueFree();
	}
}

