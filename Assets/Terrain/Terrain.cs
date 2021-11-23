using Godot;
using System.Collections.Generic;
using System;

public class Terrain : Node2D
{
    public int num_hills = 1;
    public int slice = 10;
    public int hill_range = 300;

    private Vector2 screensize;
    private List<Vector2> terrain;
    private StaticBody2D staticBody2D;
    private RigidBody2D voiture;

    private int hill_count = 0;

    [Export]
    private PackedScene gas;

    [Export]
    private PackedScene coin;

    [Export]
    private PackedScene bomb;

    private Resource texture = GD.Load("res://Assets/Terrain/DirtBG.png");

    private CanvasLayer HUD;
    private Node PlayerSettings;
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        HUD = GetNode<CanvasLayer>("/root/Hud");
        PlayerSettings = GetNode("/root/PlayerSettings");
        HUD.Scale = new Vector2(1,1);
        RichTextLabel coinLabel = HUD.GetChild<RichTextLabel>(3);
        coinLabel.Call("updateLabel");

        staticBody2D = GetNode<StaticBody2D>("Floor");
        voiture = GetNode<RigidBody2D>("Voiture");
        GD.Randomize();
        terrain = new List<Vector2>();
        screensize = GetViewport().GetVisibleRect().Size;
        screensize.x *= 1.5f;
        var start_y = 380;
        terrain.Add(new Vector2(0, -start_y));
        add_hills();
        GetNode<Polygon2D>("Polygon2D").Polygon = GetNode<CollisionPolygon2D>("Floor/CollisionPolygon2D").Polygon;
        
    }

    public void add_hills() {
        var random = GD.Randi()  % 2;
        var randomPlus3 = random + 3;
        var pourcent = (randomPlus3/2);
        var hill_width = screensize.x * pourcent / num_hills;
        int hill_slices = (int) hill_width / slice;
        Vector2 start = terrain[terrain.Count-1];
        List<Vector2> poly = new List<Vector2>(); 
        foreach (var i in GD.Range(num_hills))
        {
            float height =  GD.Randi()  % hill_range;
            start.y -= height;
            foreach (var j in GD.Range(0, hill_slices))
            {
                Vector2 hill_point = new Vector2();
                hill_point.x = start.x + j * slice + hill_width*i;
                hill_point.y = start.y + height * (float) Math.Cos(2 * Math.PI / hill_slices * j);
                if ((int)(hill_slices/2) == j) {
                    addCoin();
                }
                terrain.Add(hill_point);
                poly.Add(hill_point);
            }
            start.y += height;
        }
        var shape = new CollisionPolygon2D();
        var ground = new Polygon2D();
        staticBody2D.AddChild(shape);
        poly.Add(new Vector2(terrain[terrain.Count-1].x, screensize.y));
        poly.Add(new Vector2(start.x, screensize.y));
        shape.Polygon = poly.ToArray();
        ground.Polygon = poly.ToArray();
        ground.Texture = (Texture) texture;
        AddChild(ground);
        if (hill_count % 5 != 0) { 
            addCoin();
        } else {
            addGas();
        }
        hill_count++;
        
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
       if (terrain[terrain.Count-1].x < voiture.Position.x + screensize.x) {
           add_hills();
       }
    }

    public void SpawnBomb(Vector2 position) {
        var bombInstanciate = bomb.Instance<RigidBody2D>();
        position.y -= 500;
        position.Normalized();
        bombInstanciate.Position = position;
        AddChild(bombInstanciate);
    }

    private void addCoin() {

        int[] coinsValues = {5,10,25,50};
        Random random = new Random();
        int currentCoins = coinsValues[random.Next(0, coinsValues.Length)];
        
        var coinInstanciate = coin.Instance<Area2D>();
        coinInstanciate.Set("coinValue", currentCoins);

        Resource coinTexture = GD.Load(String.Format("res://Assets/Pickups/Coin{0}.png",currentCoins));
        coinInstanciate.GetChild<Sprite>(0).Texture = (Texture) coinTexture;
        coinInstanciate.Position = new Vector2(terrain[terrain.Count-1].x, terrain[terrain.Count-1].y-200);
        AddChild(coinInstanciate);
    }

    private void addGas() {
        var gasInstanciate = gas.Instance<Area2D>();
        gasInstanciate.Position = new Vector2(terrain[terrain.Count-1].x, terrain[terrain.Count-1].y-200);
        AddChild(gasInstanciate);
    }

}
