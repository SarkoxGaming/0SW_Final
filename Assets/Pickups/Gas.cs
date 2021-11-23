using Godot;
using System;

public class Gas : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    ProgressBar bar;
    public override void _Ready()
    {
        bar = GetNode<ProgressBar>("/root/Hud/ProgressBar");
    }

    public void _on_Gas_body_entered(Node2D body) {
        if (body.Name != "Voiture") return;
        bar.Value = bar.MaxValue;
        body.Call("GasPickup");
        QueueFree();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
