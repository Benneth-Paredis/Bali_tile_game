using Godot;
using System;
using System.Collections.Generic;


public class Game_board : Spatial
{
	PackedScene emptyTile;
	PackedScene banana_farm_tile;
	PackedScene rice_farm_tile;
	PackedScene rice_tile;
	PackedScene mountain_tile;
	float tileHeight = 1;

	AudioStreamPlayer audioStreamPlayer;

	public Dictionary<(int, int), string> occupiedPositions = new Dictionary<(int, int), string>(); // The dictionary consists of a position tuple (x,z) and a string of the type
    //Width of the hexagon tile.
    public float tileApothem = 0.866f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Get all tile scenes
		emptyTile = (PackedScene)ResourceLoader.Load("res://Scenes/Tiles/Empty_tile.tscn");
		banana_farm_tile = (PackedScene)ResourceLoader.Load("res://Scenes/Tiles/Banana_farm_tile.tscn");
		rice_tile = (PackedScene)ResourceLoader.Load("res://Scenes/Tiles/Rice_tile.tscn");
		mountain_tile = (PackedScene)ResourceLoader.Load("res://Scenes/Tiles/Mountain_tile.tscn");


		//Get audio player
		audioStreamPlayer = (AudioStreamPlayer)GetNode("AudioStreamPlayer");
		
		spawnTile("empty_tile", 0, 0);
		spawnTile("empty_tile", -3, -4);
		spawnTile("empty_tile", 5, 2);

	}
	//Checks if the position in hex-coordinates contains a tile
	public bool posHasTile(int xHex, int zHex)
	{
		return occupiedPositions.ContainsKey((xHex, zHex)); // returns if key is in occupied positions
	}
	//Spawn a tile given the tile type and its hex-coordinates
	public void spawnTile(string tile_type, int xHex, int zHex)
	{
		//Hex coordinates converted to spatial 3D coordinates
		Vector3 position = hex_coordinates(xHex, zHex);

		Tile newTile;

		if(!posHasTile(xHex, zHex))
		{
			switch (tile_type)
			{
				case "empty_tile":
					occupiedPositions.Add((xHex, zHex), "empty_tile");
					newTile = emptyTile.Instance<Empty_tile>();
					newTile.xHex = xHex;
					newTile.zHex = zHex;
					newTile.Translation = new Vector3(position.x, 0, position.z);
					AddChild(newTile);
					break;
					
				case "banana_farm_tile":
					occupiedPositions.Add((xHex, zHex), "banana_farm_tile");
					newTile = banana_farm_tile.Instance<Banana_farm_tile>();
					newTile.xHex = xHex;
					newTile.zHex = zHex;
					newTile.Translation = new Vector3(position.x, 0, position.z);
					AddChild(newTile);
					break;
					
				case "rice_farm_tile":
					occupiedPositions.Add((xHex, zHex), "rice_farm_tile");
					newTile = rice_farm_tile.Instance<Rice_farm_tile>();
					newTile.xHex = xHex;
					newTile.zHex = zHex;
					newTile.Translation = new Vector3(position.x, 0, position.z);
					AddChild(newTile);
					break;
				case "rice_tile":
					occupiedPositions.Add((xHex, zHex), "rice_tile");
					newTile = rice_tile.Instance<Rice_tile>();
					newTile.xHex = xHex;
					newTile.zHex = zHex;
					newTile.Translation = new Vector3(position.x, 0, position.z);
					AddChild(newTile);
					break;
				case "mountain_tile":
					occupiedPositions.Add((xHex, zHex), "mountain_tile");
					newTile = mountain_tile.Instance<Mountain_tile>();
					newTile.xHex = xHex;
					newTile.zHex = zHex;
					newTile.Translation = new Vector3(position.x, 0, position.z);
					AddChild(newTile);
					break;
			}
		}
	}
		
	public void click_empty_tile(int xHex,int zHex)
	{
		//Remove the empty tile from the occupied positions
		occupiedPositions.Remove((xHex, zHex));
		//Type of tile that will be spawned
		string spawn_tile_type = "banana_farm_tile";
		//Random number to decide which tile wil be spawned
		int random_number = new Random().Next(0, 101);

		if(random_number <= 10)
		{
			spawn_tile_type = "banana_farm_tile";
		}
		if(10 < random_number && random_number <= 20)
		{
			spawn_tile_type = "mountain_tile";
		}
		if(20 < random_number)
		{
			spawn_tile_type = "rice_tile";
		}

		spawnTile(spawn_tile_type, xHex, zHex);

		spawnTile("empty_tile", xHex + 1, zHex);
		spawnTile("empty_tile", xHex - 1, zHex);

		if((Math.Sign(zHex) * zHex) % 2 == 0) // even rij
		{
			spawnTile("empty_tile", xHex - 1, zHex + 1);
			spawnTile("empty_tile", xHex - 1, zHex - 1);
		}
        else // oneven rij
        {
            spawnTile("empty_tile", xHex + 1, zHex + 1);
            spawnTile("empty_tile", xHex + 1, zHex - 1);
        }


		spawnTile("empty_tile", xHex, zHex + 1);
		spawnTile("empty_tile", xHex, zHex - 1);

		audioStreamPlayer.Play();
	}
	
	public Vector3 hex_coordinates(int xHex, int zHex)
	{
        float xPos = 2 * xHex * tileApothem + (Math.Sign(zHex) * zHex) % 2 * tileApothem;
		float yPos = 0;
		float zPos = 1.5f * zHex * tileHeight;
		return new Vector3(xPos, yPos, zPos);
	}





	// Work in progress
	/*public int count_field_size(int xHex, int yHex)
	{
		if(occupiedPositions.ContainsKey((xHex, yHex)))
		{
            string tileTipe = occupiedPositions[(xHex, yHex)];
			GD.Print("Count tiles around: ", tileTipe);
        }
		else
		{
            GD.Print("Error: No tile on this position to check field size");
        }

        switch (tileTipe)
		{
			case "Banana_farm_tile":
				string fieldTipe = "Banana_tile";
				break;
            case "Rice_farm_tile":
                string fieldTipe = "Rice_tile";
				break;
        }
        return 0;
	}
	*/





}
