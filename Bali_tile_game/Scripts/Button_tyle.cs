using Godot;
using System;

public class Button_tyle : Button
{
    public string tyleType;
    Game_board game_board;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        game_board = GetNode<Game_board>("/root/Main_scene/game_board");

    }
    public void _on_button_up()
    {
        game_board.selectedTyle = game_board.playersTileOptions[game_board.playerTurn][int.Parse(this.GetName())];
        game_board.tyleSelected = true;
        GD.Print("selected: ", game_board.selectedTyle);
    }
    
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
