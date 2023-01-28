using Godot;
using System;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

public class Main : Spatial
{
    Game_board game_board;
    RichTextLabel score_display;
    String temporary_score_display;
    DynamicFont score_style;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        game_board = GetNode<Game_board>("/root/Main_scene/game_board");
        score_display = GetNode<RichTextLabel>("/root/Main_scene/UserInterface/Score");
        display_updated_score();
    }

    public void display_updated_score()
    {
        score_display.BbcodeText = "";
        temporary_score_display = "[color=#af5f0c]Score \n\n";
        for (int i = 0; i < game_board.playerList.Count; i++)
        {
            if (game_board.playerTurn == i)
            {
                temporary_score_display += "[color=#f2c22e]";
                temporary_score_display += String.Format("Player {0}: {1} points\n", i, game_board.playerList[i].score);
                temporary_score_display += "[/color]";
            }
            else
            {
                temporary_score_display += String.Format("Player {0}: {1} points\n", i, game_board.playerList[i].score);
            }
        }
        score_display.AppendBbcode(temporary_score_display);
    }
}
