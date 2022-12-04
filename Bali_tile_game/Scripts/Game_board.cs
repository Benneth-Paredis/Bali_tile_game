using Godot;
using System;

public class Game_board : Spatial
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    PackedScene emptyTile;

    //With of the hexagon tile.
    public float tileApothem = 0.866f;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        emptyTile = (PackedScene)ResourceLoader.Load("res://Scenes/Empty_tile.tscn");
        Spatial newTile = (Spatial)emptyTile.Instance();
        AddChild(newTile);
    }
    
    public void spawnTile(string tile_type, float xPos, float zPos)
    {
        //checks if the tile type is empty tile and spawns new empty tile ar position (xPos, yPos).
        if(tile_type == "empty_tile")
        {
            Spatial newTile = (Spatial)emptyTile.Instance();
            AddChild(newTile);
            newTile.Translation = new Vector3(xPos, 0, zPos);
        }
    }
}
