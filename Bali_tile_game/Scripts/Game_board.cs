using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public List<List<string>> playersTileOptions = new List<List<string>>();
    public bool tyleSelected = false;
    public string selectedTyle;
    Main main;


    AudioStreamPlayer audioStreamPlayer;

    //Width of the hexagon tile.
    public float tileApothem = 0.866f;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        main = (Main)GetParent();

        //Get all tile scenes
        emptyTile = (PackedScene)ResourceLoader.Load("res://Scenes/Tiles/Empty_tile.tscn");
        banana_farm_tile = (PackedScene)ResourceLoader.Load("res://Scenes/Tiles/Banana_farm_tile.tscn");
        rice_farm_tile = (PackedScene)ResourceLoader.Load("res://Scenes/Tiles/Rice_farm_tile.tscn");
        rice_tile = (PackedScene)ResourceLoader.Load("res://Scenes/Tiles/Rice_tile.tscn");
        banana_tile = (PackedScene)ResourceLoader.Load("res://Scenes/Tiles/Banana_tile.tscn");
        mountain_tile = (PackedScene)ResourceLoader.Load("res://Scenes/Tiles/Mountain_tile.tscn");
        player = (PackedScene)ResourceLoader.Load("res://Scenes/Player.tscn");
        

        // adding players
        player0 = player.Instance<Player>();
        AddChild(player0);
        player1 = player.Instance<Player>();
        AddChild(player1);
        playerList = new List<Player>
        {
            player0,
            player1
        };

        // giving 3 tiles to choose out per player
        GD.Print("Tiles per player: ");
        for (int i = 0; i < playerList.Count; i++)
        {
            List<string> innerList = new List<string>();
            for (int j = 0; j < 3; j++)
            {
                innerList.Add(new_random_tyle());
            }
            playersTileOptions.Add(innerList);
        }

        // printing the result
        foreach (var innerList in playersTileOptions)
        {
            GD.Print("");
            foreach (string str in innerList)
            {
                GD.Print(str + " ");
            }
            
        }

        // initialising turns
        playerTurn = 0;
        GD.Print("Player:", playerTurn, " is playing");

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

    public void click_empty_tile(int xHex, int zHex, string spawn_tile_type)
    {
        //Remove the empty tile from the occupied positions
        occupiedPositions.Remove((xHex, zHex));
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
    
    public string new_random_tyle()
    {
        string spawn_tile_type = "";
        //Random number to decide which tile wil be spawned
        int random_number = new Random().Next(0, 101);

        if (random_number <= 20)
        {
            spawn_tile_type = "banana_farm_tile";
        }
        if (20 < random_number && random_number <= 30)
        {
            spawn_tile_type = "mountain_tile";
        }
        if (30 < random_number && random_number <= 50)
        {
            spawn_tile_type = "rice_farm_tile";
        }
        if (50 < random_number && random_number <= 75)
        {
            spawn_tile_type = "banana_tile";
        }
        if (75 < random_number)
        {
            spawn_tile_type = "rice_tile";
        }
        if (spawn_tile_type == "")
        {
            GD.Print("Error in chosing tiletype in function: new_tyle_option_in_menu()");
        }
        return spawn_tile_type;
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
        bool adjacentFarmFound;
        bool unfinishedFarm;
        List<(int, int)> possibleFarmPositions = adjacent_tiles(xHex, zHex);
        List<(int, int)> adjacentTilesOfAdjacentFarm;
        List<(int, int)> ownedFarms;


        for (int i = 0; i < 6; i++)
        {
            adjacentFarmFound = false;
            unfinishedFarm = false;
            ownedFarms = new List<(int, int)> ();
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
         
            adjacentTilesOfAdjacentFarm = adjacent_tiles(possibleFarmPositions[i].Item1, possibleFarmPositions[i].Item2);
            if (adjacentFarmFound)
            {
                // checking if it is surrounded
                if (surroundedFarm(possibleFarmPositions[i].Item1, possibleFarmPositions[i].Item2))
                {
                    // find farm owner
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
                        count_farm_size(possibleFarmPositions[i].Item1, possibleFarmPositions[i].Item2, farmOwner, ownedFarms);
                        farmsize = ownedFarms.Count;
                        // check if the whole farm is surrounded
                        int j = 0;
                        bool wholeFarmSurrounded = true;
                        
                        while (j < farmsize & wholeFarmSurrounded)
                        {
                            wholeFarmSurrounded = surroundedFarm(ownedFarms[j].Item1, ownedFarms[j].Item2);
                            j++;
                        }
                        if (wholeFarmSurrounded)
                        {
                            GD.Print("Found group of ", farmsize," finished farm(s)! Of type: ", tileTipeAdjacentTtile, " from player ", playerList.IndexOf(farmOwner));
                            visitedFields = new List<(int, int)>();
                            foreach ((int, int) farmPosition in ownedFarms)
                            {
                                count_field_size(farmPosition.Item1, farmPosition.Item2, ownedFarms, visitedFields);
                            }
                            int fieldSize = visitedFields.Count;
                            GD.Print("Field size: ", fieldSize);
                            add_player_score(farmOwner, fieldSize * farmsize);
                            foreach((int,int) finishedFarm in ownedFarms)
                            {
                                farmOwner.ownedFarms.Remove(finishedFarm);
                            }                                                     
                        }
                    }
                    else
                    {
                        GD.Print("Nobody owns this fished farm");
                    }

                }
            }
        }

    }
    public void count_field_size(int xHex, int zHex, List<(int, int)> ownedFarms, List<(int, int)> visitedFields)
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
            return;
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

        backtrackingFieldCount(xHex, zHex, fieldTipe, tileTipe, visitedFields, ownedFarms);
    }
    public void count_farm_size(int xHex, int zHex, Player farmer, List<(int, int)> ownedFarms)
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
            return;
        }
        backtrackingFarmCount(xHex, zHex, tileTipe, ownedFarms, farmer);
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
            if (occupiedPositions.ContainsKey(surrounding_positions[i]))
            {
                if (fieldTipe == occupiedPositions[surrounding_positions[i]] & !visitedFields.Contains(surrounding_positions[i]))
                {
                    backtrackingFieldCount(surrounding_positions[i].Item1, surrounding_positions[i].Item2, fieldTipe, tileTipe, visitedFields, visitedFarms);
                }
                else
                {
                    if (tileTipe == occupiedPositions[surrounding_positions[i]] & !visitedFarms.Contains(surrounding_positions[i]))
                    {
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
    }
    public void backtrackingFarmCount(int xHex, int zHex, string tileTipe, List<(int, int)> ownedFarms, Player farmer)
    {
        // Backtracking algoritm that counts adjecent farms

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
        }
        else
        {
            return;
        }


        List<(int, int)> surrounding_positions = adjacent_tiles(xHex, zHex);
        for (int i = 0; i < 6; i++)
        {
            if (occupiedPositions.ContainsKey(surrounding_positions[i]))
            {

                if (tileTipe == occupiedPositions[surrounding_positions[i]] & !ownedFarms.Contains(surrounding_positions[i]))
                {
                    backtrackingFarmCount(surrounding_positions[i].Item1, surrounding_positions[i].Item2, tileTipe, ownedFarms, farmer);
                }
            }
        }
    }
    public bool surroundedFarm(int xHex, int zHex)
    {
        // returns true if farm is surrounded
        bool unfinishedFarm = false;
        List<(int,int)> adjacentTiles = adjacent_tiles(xHex, zHex);
            int j = 0;
            while (j < 6 & !unfinishedFarm)
            {
                if (occupiedPositions.ContainsKey(adjacentTiles[j]))
                {                
                    if (occupiedPositions[adjacentTiles[j]] == "empty_tile")
                    {
                        unfinishedFarm = true;
                    }
                }
                j++;
            }
        return !unfinishedFarm;
        }
    public void add_player_score(Player player, int score)
    {
        player.score += score;
        GD.Print("Gained points: ", score);
        GD.Print("Total score of player ", playerList.IndexOf(player), " = ", player.score, " points");
        main.display_point_scored(player, score);
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
        // give player new tile

        playersTileOptions[playerTurn][playersTileOptions[playerTurn].IndexOf(selectedTyle)] = new_random_tyle();
        // changes playerturn to next player
        if (playerTurn == playerList.Count - 1)
        {
            playerTurn = 0;
        }
        else
        {
            playerTurn++;
        }

        // setting variables to initial value
        tyleSelected= false;
        selectedTyle = "";

        // displaying result
        main.display_updated_score();
        main.display_tyle_selection_menu();
        
    }
    
}
