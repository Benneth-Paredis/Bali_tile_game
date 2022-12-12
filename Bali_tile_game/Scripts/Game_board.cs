using Godot;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.ConstrainedExecution;

public class Game_board : Spatial
{
    PackedScene emptyTile;
    PackedScene banana_farm_tile;
    PackedScene rice_farm_tile;
    PackedScene rice_tile;
    PackedScene banana_tile;
    PackedScene mountain_tile;
    PackedScene player;

    float tileHeight = 1;
    Player player0;
    Player player1;
    public Dictionary<(int, int), string> occupiedPositions = new Dictionary<(int, int), string>(); // The dictionary consists of a position tuple (x,z) and a string of the type
    public List<(int, int)> visitedFields = new List<(int, int)>();
    public int playerTurn;
    public List<Player> playerList;


    AudioStreamPlayer audioStreamPlayer;

    //Width of the hexagon tile.
    public float tileApothem = 0.866f;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //Get all tile scenes
        emptyTile = (PackedScene)ResourceLoader.Load("res://Scenes/Tiles/Empty_tile.tscn");
        banana_farm_tile = (PackedScene)ResourceLoader.Load("res://Scenes/Tiles/Banana_farm_tile.tscn");
        rice_farm_tile = (PackedScene)ResourceLoader.Load("res://Scenes/Tiles/Rice_farm_tile.tscn");
        rice_tile = (PackedScene)ResourceLoader.Load("res://Scenes/Tiles/Rice_tile.tscn");
        banana_tile = (PackedScene)ResourceLoader.Load("res://Scenes/Tiles/Banana_tile.tscn");
        mountain_tile = (PackedScene)ResourceLoader.Load("res://Scenes/Tiles/Mountain_tile.tscn");
        player = (PackedScene)ResourceLoader.Load("res://Scenes/Player.tscn");

        playerTurn = 0;
        GD.Print("Player:", playerTurn, " is playing");
        player0 = player.Instance<Player>();
        AddChild(player0);
        player1 = player.Instance<Player>();
        AddChild(player1);
        playerList = new List<Player>
    {
        player0,
        player1
    };

        //Get audio player
        audioStreamPlayer = (AudioStreamPlayer)GetNode("AudioStreamPlayer");

        spawnTile("empty_tile", 0, 0);
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

        if (!posHasTile(xHex, zHex))
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
                    playerList[playerTurn].ownedFarms.Add((xHex, zHex));
                    GD.Print("Ownes ", playerList[playerTurn].ownedFarms.Count, " farm(s)");
                    break;

                case "rice_farm_tile":
                    occupiedPositions.Add((xHex, zHex), "rice_farm_tile");
                    newTile = rice_farm_tile.Instance<Rice_farm_tile>();
                    newTile.xHex = xHex;
                    newTile.zHex = zHex;
                    newTile.Translation = new Vector3(position.x, 0, position.z);
                    AddChild(newTile);
                    playerList[playerTurn].ownedFarms.Add((xHex, zHex));
                    GD.Print("Ownes ", playerList[playerTurn].ownedFarms.Count, " farm(s)");
                    break;
                case "rice_tile":
                    occupiedPositions.Add((xHex, zHex), "rice_tile");
                    newTile = rice_tile.Instance<Rice_tile>();
                    newTile.xHex = xHex;
                    newTile.zHex = zHex;
                    newTile.Translation = new Vector3(position.x, 0, position.z);
                    AddChild(newTile);
                    break;
                case "banana_tile":
                    occupiedPositions.Add((xHex, zHex), "banana_tile");
                    newTile = banana_tile.Instance<Banana_tile>();
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

    public void click_empty_tile(int xHex, int zHex)
    {
        //Remove the empty tile from the occupied positions
        occupiedPositions.Remove((xHex, zHex));
        //Type of tile that will be spawned
        string spawn_tile_type = "banana_farm_tile";
        //Random number to decide which tile wil be spawned
        int random_number = new Random().Next(0, 101);
        // player is defined


        if (random_number <= 10)
        {
            spawn_tile_type = "banana_farm_tile";
        }
        if (10 < random_number && random_number <= 20)
        {
            spawn_tile_type = "mountain_tile";
        }
        if (20 < random_number && random_number <= 30)
        {
            spawn_tile_type = "rice_farm_tile";
        }
        if (30 < random_number && random_number <= 60)
        {
            spawn_tile_type = "banana_tile";
        }
        if (60 < random_number)
        {
            spawn_tile_type = "rice_tile";
        }

        spawnTile(spawn_tile_type, xHex, zHex);

        spawnTile("empty_tile", xHex + 1, zHex);
        spawnTile("empty_tile", xHex - 1, zHex);

        if ((Math.Sign(zHex) * zHex) % 2 == 0) // even rij
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




    public void check_finished_fields(int xHex, int zHex)
    {
        string tileTipeAdjacentTtile = "None";
        int fieldsize;
        int farmsize;

        bool adjacentFarmFound = false;
        bool unfinishedFarm = false;
        List<(int, int)> possibleFarmPositions = adjacent_tiles(xHex, zHex);
        List<(int, int)> adjacentTilesOfAdjacentFarm;


        for (int i = 0; i < 6; i++)
        {
            adjacentFarmFound = false;
            unfinishedFarm = false;
            if (occupiedPositions.ContainsKey(possibleFarmPositions[i]))
            {
                tileTipeAdjacentTtile = occupiedPositions[possibleFarmPositions[i]];
            }
            else
            {
                GD.Print("Error: check_finished_fields wants tileTipe of not existing tile");
            }
            // finding adjacent farm
            switch (tileTipeAdjacentTtile)
            {
                case "banana_farm_tile":
                    adjacentFarmFound = true;
                    break;
                case "rice_farm_tile":
                    adjacentFarmFound = true;
                    break;
            }
            // checking if it is surrounded and than counting fieldsize
            adjacentTilesOfAdjacentFarm = adjacent_tiles(possibleFarmPositions[i].Item1, possibleFarmPositions[i].Item2);
            if (adjacentFarmFound)
            {
                int j = 0;
                while (j < 6 & !unfinishedFarm)
                {
                    if (occupiedPositions[adjacentTilesOfAdjacentFarm[j]] == "empty_tile")
                    {
                        unfinishedFarm = true;
                    }
                    j++;
                }
                if (!unfinishedFarm)
                {
                    Player farmOwner = playerList[0]; // assigned to avoid errors but will always be updated in while loop to correct owner
                    bool farmOwner_found = false;
                    int k = 0;
                    while (k < playerList.Count & !farmOwner_found)
                    {
                        if (playerList[k].ownedFarms.Contains(possibleFarmPositions[i]))
                        {
                            farmOwner = playerList[k];
                            farmOwner_found = true;
                        }
                        k++;
                    }
                    if (farmOwner_found)
                    {
                        List<(int, int)> ownedFarms = new List<(int, int)> { possibleFarmPositions[i] };
                        GD.Print("Found finished farm! Of type: ", tileTipeAdjacentTtile, " from player ", playerList.IndexOf(farmOwner));
                        farmsize = count_farm_size(possibleFarmPositions[i].Item1, possibleFarmPositions[i].Item2, farmOwner, ownedFarms);
                        fieldsize = count_field_size(possibleFarmPositions[i].Item1, possibleFarmPositions[i].Item2, ownedFarms);
                        farmOwner.score += fieldsize * farmsize;
                        farmOwner.ownedFarms.Remove(possibleFarmPositions[i]);
                        GD.Print("Total score of player ", playerList.IndexOf(farmOwner), " = ", farmOwner.score, " points");
                    }
                    else
                    {
                        GD.Print("Nobody owns this fished farm");
                    }

                }
            }
        }

    }
    public int count_field_size(int xHex, int zHex, List<(int, int)> ownedFarms)
    {
        // counts the field size of the position and returns it as an int
        string tileTipe;
        string fieldTipe = "None";

        // checks type of tile
        if (occupiedPositions.ContainsKey((xHex, zHex)))
        {
            tileTipe = occupiedPositions[(xHex, zHex)];
        }
        else
        {
            GD.Print("Error: No tile on this position to check field size");
            return 0;
        }

        // checks type of point tiles
        switch (tileTipe)
        {
            case "banana_farm_tile":
                fieldTipe = "banana_tile";

                break;
            case "rice_farm_tile":
                fieldTipe = "rice_tile";
                break;
        }

        if (fieldTipe == "None")
        {
            GD.Print("Error: This tile is not a defined farm");
        }

        List<(int, int)> visitedFarms = ownedFarms;
        visitedFields = new List<(int, int)>();
        backtrackingFieldCount(xHex, zHex, fieldTipe, tileTipe, visitedFields, visitedFarms);
        int fieldSize = visitedFields.Count;
        GD.Print("Fieldsize = ", fieldSize);
        return fieldSize;
    }
    public int count_farm_size(int xHex, int zHex, Player farmer, List<(int, int)> ownedFarms)
    {
        // counts the field size of the position and returns it as an int
        string tileTipe;

        // checks type of tile
        if (occupiedPositions.ContainsKey((xHex, zHex)))
        {
            tileTipe = occupiedPositions[(xHex, zHex)];
        }
        else
        {
            GD.Print("Error: No tile on this position to check field size");
            return 0;
        }

        List<(int, int)> visitedFarms = new List<(int, int)>
        {
            (xHex,zHex)
        };
        GD.Print("Farmer= ", playerList.IndexOf(farmer));
        backtrackingFarmCount(xHex, zHex, tileTipe, visitedFarms, ownedFarms, farmer);
        int farmSize = ownedFarms.Count;
        GD.Print("FarmSize = ", farmSize);
        return farmSize;
    }
    public void backtrackingFieldCount(int xHex, int zHex, string fieldTipe, string tileTipe, List<(int, int)> visitedFields, List<(int, int)> visitedFarms)
    {
        // Backtracking algoritm that counts adjecent fields

        if (occupiedPositions[(xHex, zHex)] == fieldTipe && !visitedFields.Contains((xHex, zHex)))
        {
            //GD.Print("field ",fieldCounter, " New field on tile: ", (xHex, zHex));
            visitedFields.Add((xHex, zHex));
        }

        List<(int, int)> surrounding_positions = adjacent_tiles(xHex, zHex);
        for (int i = 0; i < 6; i++)
        {
            if (fieldTipe == occupiedPositions[surrounding_positions[i]] & !visitedFields.Contains(surrounding_positions[i]))
            {
                backtrackingFieldCount(surrounding_positions[i].Item1, surrounding_positions[i].Item2, fieldTipe, tileTipe, visitedFields, visitedFarms);
            }
            else
            {
                if (tileTipe == occupiedPositions[surrounding_positions[i]] & !visitedFarms.Contains(surrounding_positions[i]))
                {
                    visitedFarms.Add(surrounding_positions[i]);
                    Player farmOwner = playerList[0]; // assigned to avoid errors but will always be updated in while loop to correct owner
                    bool farmOwner_found = false;
                    int j = 0;
                    while (j < playerList.Count & !farmOwner_found)
                    {
                        if (playerList[j].ownedFarms.Contains(surrounding_positions[i]))
                        {
                            farmOwner = playerList[j];
                            farmOwner_found = true;
                        }
                        j++;
                    }
                    if (farmOwner_found)
                    {
                        farmOwner.ownedFarms.Remove(surrounding_positions[i]);
                        GD.Print("Player: ", playerList.IndexOf(farmOwner), " lost its ", occupiedPositions[surrounding_positions[i]]);
                    }
                }
            }


        }
    }
    public void backtrackingFarmCount(int xHex, int zHex, string tileTipe, List<(int, int)> visitedFarms, List<(int, int)> ownedFarms, Player farmer)
    {
        // Backtracking algoritm that counts adjecent farms
        if (occupiedPositions[(xHex, zHex)] == tileTipe && !visitedFarms.Contains((xHex, zHex)))
        {
            Player farmOwner = playerList[0]; // assigned to avoid errors but will always be updated in while loop to correct owner
            bool farmOwner_found = false;
            int j = 0;
            while (j < playerList.Count & !farmOwner_found)
            {
                if (playerList[j].ownedFarms.Contains((xHex, zHex)))
                {
                    farmOwner = playerList[j];
                    farmOwner_found = true;
                }
                j++;
            }
            if (farmOwner_found & farmOwner == farmer)
            {
                ownedFarms.Add((xHex, zHex));
                farmer.ownedFarms.Remove((xHex, zHex));
            }
            visitedFarms.Add((xHex, zHex));
        }

        List<(int, int)> surrounding_positions = adjacent_tiles(xHex, zHex);
        for (int i = 0; i < 6; i++)
        {
            if (tileTipe == occupiedPositions[surrounding_positions[i]] & !visitedFarms.Contains(surrounding_positions[i]))
            {
                backtrackingFarmCount(surrounding_positions[i].Item1, surrounding_positions[i].Item2, tileTipe, visitedFarms, ownedFarms, farmer);
            }
        }
    }

    public List<(int, int)> adjacent_tiles(int xHex, int zHex)
    {
        // list of possible positions next to current tile in tuple of hex coordinates
        List<(int, int)> surrounding_positions = new List<(int, int)>(6);
        surrounding_positions.Add((1 + xHex, zHex + 0));
        surrounding_positions.Add((-1 + xHex, zHex + 0));
        surrounding_positions.Add((0 + xHex, zHex + 1));
        surrounding_positions.Add((0 + xHex, zHex + -1));
        if ((Math.Sign(zHex) * zHex) % 2 == 0) // even rij
        {
            surrounding_positions.Add((-1 + xHex, zHex + 1));
            surrounding_positions.Add((-1 + xHex, zHex + -1));
        }
        else // oneven rij
        {
            surrounding_positions.Add((1 + xHex, zHex + 1));
            surrounding_positions.Add((1 + xHex, zHex + -1));
        }
        return surrounding_positions;
    }
    public void next_player()
    {
        // changes playerturn to next player
        if (playerTurn == playerList.Count - 1)
        {
            playerTurn = 0;
        }
        else
        {
            playerTurn++;
        }
    }

}
