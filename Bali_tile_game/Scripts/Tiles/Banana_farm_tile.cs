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

}
