using Godot;

public class Voiture : RigidBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    Vector2 Up = new Vector2(0,-1);

    [Export]
    public int MAX_SPEED = 10;
    [Export]
    public int Acceleration = 2;

    public int distanceParcouru = 0;

    Node PlayerSettings;
    RigidBody2D wheel_r;
    RigidBody2D wheel_l;

    AudioStreamPlayer2D moteur;
    AudioStreamPlayer2D coinPickup;
    AudioStreamPlayer2D gasPickup;

    Node2D terrain;

    ProgressBar bar;
    ProgressBar nitro;
    RichTextLabel speed;
    RichTextLabel distanceLabel;

    public override void _Ready()
    {
        PlayerSettings = GetNode("/root/PlayerSettings");
        terrain = GetParent<Node2D>();
        MAX_SPEED = (int) PlayerSettings.Get("PlayerSpeedLevel") + 10;
        wheel_l = GetNode<RigidBody2D>("Wheels/Wheel_L");
        wheel_r = GetNode<RigidBody2D>("Wheels/Wheel_R");
        moteur = GetNode<AudioStreamPlayer2D>("Moteur");
        coinPickup = GetNode<AudioStreamPlayer2D>("CoinPickup");
        gasPickup = GetNode<AudioStreamPlayer2D>("GasPickup");
        moteur.Playing = true;
        Mass = (float) PlayerSettings.Call("GetCarMass");

        bar = GetNode<ProgressBar>("/root/Hud/ProgressBar");
        nitro = GetNode<ProgressBar>("/root/Hud/NitroBar");
        speed = GetNode<RichTextLabel>("/root/Hud/SpeedValue/Speed");
        distanceLabel = GetNode<RichTextLabel>("/root/Hud/distanceReachLabel/distanceReachValue");
        bar.Value = bar.MaxValue;
        nitro.Value = nitro.MaxValue;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        if (Input.IsActionPressed("ui_right") && bar.Value > bar.MinValue) {
            if (wheel_l.AngularVelocity < MAX_SPEED && wheel_r.AngularVelocity < MAX_SPEED) {
                wheel_r.AngularVelocity += Acceleration;
                wheel_l.AngularVelocity += Acceleration;
                bar.Value --;
                
            } else {
                wheel_r.AngularVelocity = MAX_SPEED;
                wheel_l.AngularVelocity = MAX_SPEED;
            }

        } else if (Input.IsActionPressed("ui_left") && bar.Value > bar.MinValue) {
            if (wheel_l.AngularVelocity > -MAX_SPEED && wheel_r.AngularVelocity > -MAX_SPEED) {
                wheel_r.AngularVelocity = -MAX_SPEED;
                wheel_l.AngularVelocity = -MAX_SPEED;
                bar.Value --;
                
            } else {
                wheel_r.AngularVelocity = -MAX_SPEED;
                wheel_l.AngularVelocity = -MAX_SPEED;
            }
            
        } 

        if (Input.IsActionPressed("ui_nitro")) {
            if (nitro.Value <= 0) return;

            wheel_l.AngularVelocity = MAX_SPEED * 3;
            wheel_r.AngularVelocity = MAX_SPEED * 3;
            nitro.Value--;
        }


        bar.GetChild<RichTextLabel>(0).Text = bar.Value.ToString();
        speed.Text = "Weel left: "+ wheel_l.AngularVelocity+"\nWeel Right: "+ wheel_r.AngularVelocity;
        setMaxDistance((int) ((Position.x-1100)/10));
    
        if (bar.Value <= 0) {
            endRace();
        }
    }

    private void setMaxDistance(int distance) {
        distanceParcouru = distanceParcouru > distance? distanceParcouru : distance;
        distanceLabel.Text = distanceParcouru.ToString();
    }

    public void CoinPickup() {
        coinPickup.Play();
    }

    public void GasPickup() {
        gasPickup.Play();
    }

    public void BombHit() {
        bar.Value /= 2;
    }

    public void _on_Area2D_body_entered(Node2D body) {
        if (body.Name != "Floor") return;
            endRace();
    }

    public void _on_BombFallingTimer_timeout() {
        terrain.Call("SpawnBomb", Position);
    }

    private void endRace() {

        var hud = GetNode<CanvasLayer>("/root/Hud");
        hud.Call("dead",(int) distanceParcouru);
        
    }
}
