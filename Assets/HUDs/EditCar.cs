using Godot;
using System;

public class EditCar : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    Node PlayerSettings;
    RichTextLabel CoinLabel; 
    RichTextLabel DistLabel;

    public override void _Ready()
    {
        GetNode<CanvasLayer>("/root/Hud").Scale = new Vector2(0,0);   
        PlayerSettings = GetNode("/root/PlayerSettings");
        CoinLabel = GetNode<RichTextLabel>("CoinLabel");
        DistLabel = GetNode<RichTextLabel>("distanceReachLabel/distanceReachValue");



        CoinLabel.Text = PlayerCoins().ToString();
        DistLabel.Text = (string) PlayerSettings.Call("getMaxDistance");
        updateAllBoost();
    }

    public int PlayerCoins() {
        return (int) PlayerSettings.Get("PlayerCoins");
    }


    public void _on_Play_pressed() {
        PlayerSettings.Call("QuickSave");
        GetTree().ChangeScene("res://Assets/Terrain/Terrain.tscn");
    }

    public void _on_SpeedUpgrade_pressed() {
        PlayerSettings.Call("addSpeedLevel");
        updateAllBoost();
        PlayerSettings.Call("QuickSave");
    }

    public void _on_GasUpgrade_pressed() {
        PlayerSettings.Call("addGasLevel");
        updateAllBoost();
        PlayerSettings.Call("QuickSave");
    }

    public void _on_GravityUpgrade_pressed() {
        PlayerSettings.Call("addGravityLevel");
        updateAllBoost();
        PlayerSettings.Call("QuickSave");
    }

    public void _on_NitroUpgrade_pressed() {
        PlayerSettings.Call("addNitroLevel");
        updateAllBoost();
        PlayerSettings.Call("QuickSave");
    }

    private void updateBuying(Control node, int savedLvl) {
        RichTextLabel lvl = node.GetChild<RichTextLabel>(0);
        RichTextLabel cost = node.GetChild<RichTextLabel>(1);
        TextureButton button = node.GetChild<TextureButton>(2);

        lvl.Text = "NIVEAU %".ReplaceN("%", savedLvl.ToString());
        int costValue = (int) (10 + Math.Pow(2, savedLvl));
        cost.Text = costValue.ToString();
        if (PlayerCoins() < costValue) {
            button.Disabled = true;
        }
        CoinLabel.Text = PlayerCoins().ToString();
    }

    private void updateAllBoost() {
        var Booster = GetTree().GetNodesInGroup("Boost");
        foreach (Control boost in Booster)
        {
            updateBuying(boost, (int) PlayerSettings.Get($"Player{boost.Name}Level"));
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
