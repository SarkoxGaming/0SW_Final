using Godot;
using System;

public class Menu : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode<Button>("VBoxContainer/StartButton").GrabFocus();
        GetNode<CanvasLayer>("/root/Hud").Scale = new Vector2(0,0);
    }

    public void _on_StartButton_pressed() {
        GetTree().ChangeScene("res://Assets/Terrain/Terrain.tscn");
    }

    public void _on_CustomCar_pressed() {
        GetTree().ChangeScene("res://Assets/HUDs/EditCar.tscn");
    }

    public void _on_QuitButton_pressed() {
        GetTree().Quit();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
