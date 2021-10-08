using Godot;
using System.Collections.Generic;
using System;

public class Terrain : Node2D
{
    public int num_hills = 1;
    public int slice = 10;
    public int hill_range = 200;

    private Vector2 screensize;
    private List<Vector2> terrain;
    private Line2D line2D;
    private StaticBody2D staticBody2D;
    private RigidBody2D voiture;

    private Resource texture = GD.Load("res://Assets/Terrain/DirtBG.png");
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        line2D = GetNode<Line2D>("Line2D");
        staticBody2D = GetNode<StaticBody2D>("StaticBody2D");
        voiture = GetNode<RigidBody2D>("Voiture");
        GD.Randomize();
        terrain = new List<Vector2>();
        screensize = GetViewport().GetVisibleRect().Size;
        var start_y = screensize.y * 3/4 + (-hill_range + GD.Randi() % hill_range * 2);
        terrain.Add(new Vector2(0, start_y));
        add_hills();

    }

    public void add_hills() {
        var hill_width = screensize.x / num_hills;
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
                line2D.AddPoint(hill_point);
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
        
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
       if (terrain[0].x < voiture.Position.x + screensize.x /2) {
           add_hills();
       }
    }
}
