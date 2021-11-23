using Godot;
using System;

public class FallingBomb : RigidBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void Explode() {
        QueueFree();
    }

    public void _on_FallingBomb_body_entered(Node body) {
        GD.Print(body.Name);
    }

    public void _on_Area2D_body_entered(Node body) {
        if (body.Name == "FallingBomb") return;
        if (body.Name == "Floor") {
            Explode();
            return;
        }
        if (body.Name == "Voiture") {
            Explode();
            body.Call("BombHit");
        }
        
        GD.Print(body.Name);
    }


//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
