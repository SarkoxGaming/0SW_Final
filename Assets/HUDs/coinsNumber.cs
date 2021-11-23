using Godot;
using System;

public class coinsNumber : RichTextLabel
{
    Node PlayerSettings;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        PlayerSettings = GetNode("/root/PlayerSettings");
    }

    public void updateLabel() {
        this.Text = PlayerSettings.Get("PlayerCoins").ToString();
    }

    public void addCoin(int quantity) {
        PlayerSettings.Call("AddCoins", quantity);
        updateLabel();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
