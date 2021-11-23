using Godot;
using System;

public class PlayerSettings : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export]
    int PlayerCoins = 0;

    [Export]
    int PlayerGasLevel = 1;

    [Export]
    int PlayerSpeedLevel = 1;

    [Export]
    int PlayerNitroLevel = 1;

    [Export]
    int PlayerGravityLevel = 1;
    int PlayerMaxDistance = 0;

    CanvasLayer HUD;
    RichTextLabel coinLabel;
    ProgressBar gas;
    TextureProgress nitro;
    RichTextLabel speedDebug;
    Boolean debugMode = false;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        HUD = GetNode<CanvasLayer>("/root/Hud");
        gas = HUD.GetChild<ProgressBar>(0);
        nitro = HUD.GetNode<TextureProgress>("NitroBar");
        speedDebug = GetNode<RichTextLabel>("/root/Hud/SpeedValue");
        LoadGame();
    }

    public Godot.Collections.Dictionary<string, object> Save()
    {
        return new Godot.Collections.Dictionary<string, object>()
        {
            { "PlayerCoins", this.PlayerCoins },
            { "PlayerGasLevel", this.PlayerGasLevel },
            { "PlayerSpeedLevel", this.PlayerSpeedLevel},
            { "PlayerGravityLevel", this.PlayerGravityLevel},
            { "PlayerNitroLevel", this.PlayerNitroLevel},
            { "PlayerMaxDistance", this.PlayerMaxDistance},
            
        };
    }

    public void AddCoins(int number) {
        PlayerCoins += number;
    }

    public void addSpeedLevel() {
        PlayerCoins -= lvlCost(PlayerSpeedLevel);;
        PlayerSpeedLevel++;
    }

    public void addGasLevel() {
        PlayerCoins -=  lvlCost(PlayerGasLevel);
        PlayerGasLevel++;
        updateGasBar();
    }

    public void addNitroLevel() {
        PlayerCoins -= lvlCost(PlayerNitroLevel);;
        PlayerNitroLevel++;
        updateNitroBar();
    }

    public void addGravityLevel() {
        PlayerCoins -=  lvlCost(PlayerGravityLevel);
        PlayerGravityLevel++;
    }

    public float GetCarMass() {
        return 300 - (this.PlayerGravityLevel*5);
    }

    public string getMaxDistance() {
        return PlayerMaxDistance.ToString();
    }

    public void setMaxDistance(int distance) {
        PlayerMaxDistance = PlayerMaxDistance > distance? PlayerMaxDistance : distance;
    }

    public void switchDebugMode() {
        debugMode = debugMode? false : true;
        gas.GetChild<RichTextLabel>(0).Visible = debugMode;
        speedDebug.Visible = debugMode;
    }

    public void QuickSave() {
        var saveGame = new File();
        saveGame.Open("user://savegame.save", File.ModeFlags.Write);
        saveGame.StoreLine(JSON.Print(Save()));
        saveGame.Close();
    }

    public void SaveGame()
    {
        QuickSave();
        GetTree().ChangeScene("res://Assets/HUDs/EditCar.tscn");
    }
    public void LoadGame()
    {
        var saveGame = new File();
        if (!saveGame.FileExists("user://savegame.save"))
            return;

          saveGame.Open("user://savegame.save", File.ModeFlags.Read);

        var nodeData = new Godot.Collections.Dictionary<string, int>((Godot.Collections.Dictionary)JSON.Parse(saveGame.GetLine()).Result);

        nodeData.TryGetValue("PlayerCoins", out int outPlayerCoins);
        nodeData.TryGetValue("PlayerGasLevel", out int outPlayerGasLevel);
        nodeData.TryGetValue("PlayerSpeedLevel", out int outPlayerSpeedLevel);
        nodeData.TryGetValue("PlayerGravityLevel", out int outPlayerGravityLevel);
        nodeData.TryGetValue("PlayerNitroLevel", out int outPlayerNitroLevel);
        nodeData.TryGetValue("PlayerMaxDistance", out int outPlayerMaxDistance);

        this.PlayerCoins = outPlayerCoins;
        this.PlayerGasLevel = outPlayerGasLevel;
        this.PlayerSpeedLevel = outPlayerSpeedLevel;
        this.PlayerGravityLevel = outPlayerGravityLevel;
        this.PlayerNitroLevel = outPlayerNitroLevel;
        this.PlayerMaxDistance = outPlayerMaxDistance;

        //int.TryParse(outPlayerMaxDistance.ToString(), out this.PlayerMaxDistance);

        updateGasBar();
        updateNitroBar();

        saveGame.Close();
    }

    private void updateGasBar() {
        gas.MaxValue = (double) (Math.Pow(2, this.PlayerGasLevel )) + 500;
    }

    private void updateNitroBar() {
        nitro.MaxValue = (double) (this.PlayerNitroLevel * 15) + 100;
    }

    private int lvlCost(int lvl) {
        return (int) (10 + Math.Pow(2, lvl));
    }
}
