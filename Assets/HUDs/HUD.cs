using Godot;
using System;

public class HUD : CanvasLayer
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    Node PlayerSettings;
    public override void _Ready()
    {
        PlayerSettings = GetNode("/root/PlayerSettings");
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public async void dead(int distanceParcouru) {

        GetNode<Popup>("DeadPopup").Show();
        await ToSignal(GetTree().CreateTimer(3), "timeout");

        GetNode<Popup>("DeadPopup").Hide();

        PlayerSettings.Call("setMaxDistance", (int) distanceParcouru);
        PlayerSettings.Call("SaveGame");
    }

}
