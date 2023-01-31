using Godot;
using System;
using System.Collections;


public class No_tile_selected_display : Control
{
    bool animationPlayed = false;

    // Called when the node enters the scene tree for the first time.
    //public override void _Ready()
    //{

    //}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        AnimationPlayer animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        if (!animationPlayed)
        {                        
            animationPlayer.Play("ScaleUpAndDown");
            animationPlayed = true;
        }
        else
        {
            if (animationPlayer.IsPlaying() == false)
            {
                QueueFree();
            }
        }
    }


    
    

}
