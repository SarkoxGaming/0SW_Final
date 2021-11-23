using Godot;
using System;

public class PauseButton : Button
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    Popup popup;
    Node PlayerSettings;
    public override void _Ready()
    {
        popup = GetNode<Popup>("../PausePopUp");
        PlayerSettings = GetNode("/root/PlayerSettings");
    }

    public void _on_PauseButton_pressed() {
        pause();
    }

    private void pause() {
        GetTree().Paused = GetTree().Paused? false: true;
        if (popup.Visible) 
            popup.Hide();
        else
            popup.Show();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("ui_pause")) {
            pause();
        } 
        if (Input.IsActionJustPressed("ui_debug")) {
            PlayerSettings.Call("switchDebugMode");
        }
        if (Input.IsActionJustPressed("ui_end")) {
            GetNode("/root/Hud").Call("dead", 0);
        }
    }
}
