using Godot;
using System;
using System.Linq;
using System.Numerics;
using static System.Net.Mime.MediaTypeNames;

public class Main : Spatial
{
    Game_board game_board;
    RichTextLabel score_display;
    String temporary_score_display;
    PackedScene extra_points_display_scene;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        game_board = GetNode<Game_board>("/root/Main_scene/game_board");
        score_display = GetNode<RichTextLabel>("/root/Main_scene/UserInterface/Score");
        extra_points_display_scene = (PackedScene)ResourceLoader.Load("res://Scenes/Extra_points_display.tscn");
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
                temporary_score_display += "[color=#ffd39b]";
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

    public void display_point_scored(Player player, int score)
    {
        Control extra_points_display_child = extra_points_display_scene.Instance<Control>();
        AddChild(extra_points_display_child);
        RichTextLabel extra_points_display = extra_points_display_child.GetNode<RichTextLabel>("extra_points_scored");
        extra_points_display.BbcodeText = "";
        extra_points_display.BbcodeText = "\n\n";
        temporary_score_display = "";
        for (int i = 0; i < game_board.playerList.Count; i++)
        {
            if (i == game_board.playerList.IndexOf(player))
            {
                temporary_score_display += "[color=#ffb90f]+ " + score + "[/color]";
            }
            temporary_score_display += "\n";
        }
        extra_points_display.AppendBbcode(temporary_score_display);
    }
}
