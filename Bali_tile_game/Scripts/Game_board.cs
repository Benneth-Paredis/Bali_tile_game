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
	float tileHeight = 1;

	//With of the hexagon tile.
	public float tileApothem = 0.866f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		emptyTile = (PackedScene)ResourceLoader.Load("res://Scenes/Empty_tile.tscn");
		banana_farm_tile = (PackedScene)ResourceLoader.Load("res://Scenes/Banana_farm_tile.tscn");
		rice_farm_tile = (PackedScene)ResourceLoader.Load("res://Scenes/Rice_farm_tile.tscn");
		
		spawnTile("empty_tile", hex_coordinates(0,0));
	}
	
	public void spawnTile(string tile_type, Vector3 position)
	{
		//checks if the tile type is empty tile and spawns new empty tile ar position (xPos, yPos).
		Tile newTile;
		
		switch (tile_type)
		{
			case "empty_tile":
				newTile = (Tile)emptyTile.Instance();
				AddChild(newTile);
				newTile.Translation = new Vector3(position.x, 0, position.z);
				newTile.xHex = (int)position.x;
				newTile.zHex = (int)position.z;
				break;
				
			case "banana_farm_tile":
				newTile = (Tile)banana_farm_tile.Instance();
				AddChild(newTile);
				newTile.Translation = new Vector3(position.x, 0, position.z);
				newTile.xHex = (int)position.x;
				newTile.zHex = (int)position.z;
				break;
				
			case "rice_farm_tile":
				newTile = (Tile)rice_farm_tile.Instance();
				AddChild(newTile);
				newTile.Translation = new Vector3(position.x, 0, position.z);
				newTile.xHex = (int)position.x;
				newTile.zHex = (int)position.z;
				break;
		}
	}
		//Spawn 6 empty tiles surrounding the clicked tile and deleting it.
		
	public void click_empty_tile(int xHex,int zHex)
	{
		spawnTile("banana_farm_tile", hex_coordinates(xHex, zHex));
//		spawnTile("empty_tile", xPos + 2*tileApothem, zPos);
//		spawnTile("empty_tile", xPos - 2*tileApothem, zPos);
//		spawnTile("empty_tile", xPos + tileApothem, zPos + 1.5f * tileHeight);
//		spawnTile("empty_tile", xPos + tileApothem, zPos - 1.5f * tileHeight);
//		spawnTile("empty_tile", xPos - tileApothem, zPos + 1.5f * tileHeight);
//		spawnTile("empty_tile", xPos - tileApothem, zPos - 1.5f * tileHeight);
		
	}
	
	public Vector3 hex_coordinates(int hexX, int hexZ)
	{
		float xPos = hexX*2*tileApothem + (hexZ%2) * tileApothem ;
		float yPos = 0;
		float zPos = hexZ * 1.5f * tileHeight;
		return new Vector3(xPos, yPos, zPos);
	}
	
}
