using Godot;
using System;
using static Game_board;

public class Empty_tile : Tile
{

	Tween tween;
    PackedScene no_tyle_selected_display;
	Main main;

    Random random = new Random();
	float random_time;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		game_board = (Game_board)GetParent();
        no_tyle_selected_display = (PackedScene)ResourceLoader.Load("res://Scenes/No_tile_selected.tscn");
		main = GetNode<Main>("/root/Main_scene");

        //Random time for the rise animation
        random_time = random.Next(0, 11) * 0.1f * 0.4f + 0.2f;
		//Rise animation
		tween = (Tween)GetNode("Tween");
		tween.InterpolateProperty(this, "translation", new Vector3(this.Translation.x, this.Translation.y - 1.0f, this.Translation.z),
		new Vector3(this.Translation.x, this.Translation.y, this.Translation.z), random_time, Tween.TransitionType.Linear, Tween.EaseType.InOut);
		tween.Start();
	}

	//Function that gets called when a hex tile is clicked.
	private void _on_Area_input_event(object camera, object @event, Vector3 position, Vector3 normal, int shape_idx)
	{
		//Checks if it is a mouse button click
		if(@event is InputEventMouseButton inputEventMouseButton)
		{
			//Left mouse button click
			if(inputEventMouseButton.Pressed == true && inputEventMouseButton.ButtonIndex == 1)
			{
				if (game_board.tyleSelected)
				{
					String spawn_tile_type = game_board.selectedTyle;

					game_board.click_empty_tile(this.xHex, this.zHex, spawn_tile_type);
					QueueFree();
					game_board.check_finished_fields(this.xHex, this.zHex);
					game_board.next_player();
					GD.Print("");
					GD.Print("Player: ", game_board.playerTurn, " is playing");
					GD.Print("Select new tile to place: ");
				}
				else
				{
                    Control extra_points_display_child = no_tyle_selected_display.Instance<Control>();
                    main.AddChild(extra_points_display_child);
                    GD.Print("No tyle selected");
				}
            }
            //Right mouse button click
            if (inputEventMouseButton.Pressed == true && inputEventMouseButton.ButtonIndex == 2)
			{
				GD.Print("Right mouse button click");
				game_board.occupiedPositions.Remove((this.xHex, this.zHex));
				QueueFree();
			}
		}
	}
}

