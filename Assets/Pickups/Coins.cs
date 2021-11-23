using Godot;
using System;

public class Coins : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export]
    private int coinValue = 0;

    RichTextLabel coinLabel;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        coinLabel = GetNode<RichTextLabel>("/root/Hud/coinsNumber");  
    }

    public void _on_Coin5_body_entered(Node2D body) {
        if (body.Name != "Voiture") return;
        
        coinLabel.Call("addCoin", coinValue);
        body.Call("CoinPickup");

        QueueFree();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
