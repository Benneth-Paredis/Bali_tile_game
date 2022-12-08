using Godot;
using System;

public class Player : Node
{
    public int score;
    public bool isTurn;
    public String playerColor;

    public Player(String playerColor)
    {
        this.playerColor = playerColor;
    }
    public override void _Ready()
    {
        
    }
}
