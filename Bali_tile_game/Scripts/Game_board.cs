using Godot;
using System;

public class Game_board : Spatial
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	PackedScene emptyTile;
	PackedScene banana_farm_tile;
	PackedScene rice_farm_tile;

	//With of the hexagon tile.
	public float tileApothem = 0.866f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		emptyTile = (PackedScene)ResourceLoader.Load("res://Scenes/Empty_tile.tscn");
		banana_farm_tile = (PackedScene)ResourceLoader.Load("res://Scenes/Banana_farm_tile.tscn");
		rice_farm_tile = (PackedScene)ResourceLoader.Load("res://Scenes/Rice_farm_tile.tscn");
		
		spawnTile("empty_tile", 0, 0);
	}
	
	public void spawnTile(string tile_type, float xPos, float zPos)
	{
		//checks if the tile type is empty tile and spawns new empty tile ar position (xPos, yPos).
		Spatial newTile;
		
		switch (tile_type)
		{
			case "empty_tile":
				newTile = (Spatial)emptyTile.Instance();
				AddChild(newTile);
				newTile.Translation = new Vector3(xPos, 0, zPos);
				break;
				
			case "banana_farm_tile":
				newTile = (Spatial)banana_farm_tile.Instance();
				AddChild(newTile);
				newTile.Translation = new Vector3(xPos, 0, zPos);
				break;
				
			case "rice_farm_tile":
				newTile = (Spatial)rice_farm_tile.Instance();
				AddChild(newTile);
				newTile.Translation = new Vector3(xPos, 0, zPos);
				break;
		}
	}
		//Spawn 6 empty tiles surrounding the clicked tile and deleting it.
		
	public void click_empty_tile(float xPos,float zPos)
	{
		spawnTile("banana_farm_tile", xPos, zPos);
//		game_board.spawnTile("empty_tile", this.Translation.x + 2*game_board.tileApothem, this.Translation.z);
//		game_board.spawnTile("empty_tile", this.Translation.x - 2*game_board.tileApothem, this.Translation.z);
//		game_board.spawnTile("empty_tile", this.Translation.x + game_board.tileApothem, this.Translation.z + 1.5f);
//		game_board.spawnTile("empty_tile", this.Translation.x + game_board.tileApothem, this.Translation.z - 1.5f);
//		game_board.spawnTile("empty_tile", this.Translation.x - game_board.tileApothem, this.Translation.z + 1.5f);
//		game_board.spawnTile("empty_tile", this.Translation.x - game_board.tileApothem, this.Translation.z - 1.5f);
		
	}
}
