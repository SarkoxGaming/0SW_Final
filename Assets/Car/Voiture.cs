using Godot;

public class Voiture : RigidBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    Vector2 Up = new Vector2(0,-1);

    const int GRAVITY = 20;
    const int MAX_FALL_SPEED = 200;
    const int MAX_SPEED = 80;
    public int Acceleration = 20;

    RigidBody2D wheel_r;
    RigidBody2D wheel_l;

     public override void _Ready()
    {
        wheel_l = GetNode<RigidBody2D>("Wheels/Wheel_L");
        wheel_r = GetNode<RigidBody2D>("Wheels/Wheel_R");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {

        //Velocity.x = Velocity.Clamped(MAX_SPEED).x;

        if (Input.IsActionPressed("ui_right")) {
            wheel_r.AngularVelocity += Acceleration;
            wheel_l.AngularVelocity += Acceleration;
            
        } else if (Input.IsActionPressed("ui_left")) {
            wheel_r.AngularVelocity -= Acceleration;
            wheel_l.AngularVelocity -= Acceleration;
        } else {
        }
    }
}
