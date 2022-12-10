using Godot;
using System;
using System.Collections.Generic;

public class Player : Node
{
    public int score;
    public bool isTurn;
    public String playerColor;
    public List<(int, int)> ownedFarms;

    public Player(String playerColor)
    {
        this.playerColor = playerColor;
    }
    public override void _Ready()
    {
        
    }
}
