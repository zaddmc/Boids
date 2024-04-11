using Godot;
using System;
using System.Collections.Generic;

public partial class Overlord : Node2D {
    [Export]
    static public int SpriteCount = 100;
    [Export]
    static public bool EndlessScreen = true;

    static public float Radius = 250;

    static public double AlignWeight = 0;
    static public double CohesionWeight = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        Sprites = new List<OwnSprite>();
        Random random = new Random();
        Node2D node = GetNode<Node2D>(".");

        for (int i = 0; i < SpriteCount; i++) {


            OwnSprite sprite = new OwnSprite();
            sprite.Sprite.Position = new Vector2(random.NextSingle() * 1000, random.NextSingle() * 500);

            Sprites.Add(sprite);
            node.AddChild(sprite.Sprite);

        }
    }
    static public List<OwnSprite> Sprites { get; set; }

    public override void _Draw() {
        DrawArc(Sprites[0].Sprite.Position, Radius, Sprites[0].Sprite.Rotation - MathF.PI / 3, Sprites[0].Sprite.Rotation + MathF.PI / 3, 20, Colors.AliceBlue, 5);
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
        RotationNormalizer();
        MoveSprites();
        Alignment();
        Cohesion();
        GetSliderValue();
        Test();
    }

    public void MoveSprites() {
        foreach (var sprite in Sprites) {
            sprite.Sprite.Rotate(sprite.RotationalChange);
            sprite.RotationalChange = 0;
            RotationNormalizer();
            Vector2 newOffset = new(sprite.VelocityNUM * MathF.Cos(sprite.Sprite.Rotation), sprite.VelocityNUM * MathF.Sin(sprite.Sprite.Rotation));
            sprite.VelocityVEC = newOffset * 2;

            //sprite.Sprite.Position += newOffset;
            sprite.Sprite.Translate(newOffset);
            if (EndlessScreen) {
                Window window = GetViewport().GetWindow();
                if (sprite.Sprite.Position.X >= window.Size.X)
                    sprite.Sprite.Position -= new Vector2(window.Size.X, 0);
                if (sprite.Sprite.Position.X <= 0)
                    sprite.Sprite.Position += new Vector2(window.Size.X, 0);
                if (sprite.Sprite.Position.Y >= window.Size.Y)
                    sprite.Sprite.Position -= new Vector2(0, window.Size.Y);
                if (sprite.Sprite.Position.Y <= 0)
                    sprite.Sprite.Position += new Vector2(0, window.Size.Y);
            }
        }
    }
    public void RotationNormalizer() {
        foreach (var sprite in Sprites) {
            if (sprite.Sprite.Rotation > Mathf.Tau)
                sprite.Sprite.Rotation -= Mathf.Tau;
            else if (sprite.Sprite.Rotation < 0)
                sprite.Sprite.Rotation += MathF.Tau;
        }
    }
    public void Alignment() {//find hvilke boids der er i radius. if is in range then.  gem deres alignment/retning i xy og antal. bagefter divider med x og y med antal. 
        foreach (var mainsprite in Sprites) {
            float procent = 0.01f;
            uint amount = 0;
            float cummrotation = 0;
            foreach (var other in Sprites) {
                if (mainsprite.IsInRange(other)) {
                    cummrotation += other.Sprite.Rotation % Mathf.Tau;
                    amount++;
                }
            }
            float average = cummrotation / amount;
            //mainsprite.Sprite.Rotate((average - mainsprite.Sprite.Rotation) * procent * (float)AlignWeight);
            mainsprite.RotationalChange += (average - mainsprite.Sprite.Rotation) * procent * (float)AlignWeight;
            if (mainsprite.Equals(Sprites[0])) {
            }
        }

    }
    public void Cohesion() {//position of the object. its vector. then make the angle to the middle point. voila
        foreach (var mainsprite in Sprites) {
            float procent = 0.01f;
            uint amount = 0;
            Vector2 cummposition = new(0, 0);
            foreach (var other in Sprites) {
                if (mainsprite.IsInRange(other)) {
                    cummposition = mainsprite.Sprite.Position;
                    amount++;
                }
                Vector2 averagePosition = new(cummposition[0], cummposition[1]);//[0] means x and [1] means y
                float step1 = mainsprite.Sprite.Position.AngleToPoint(averagePosition);
                float step2 = (mainsprite.Sprite.Rotation + step1) % MathF.Tau;
                if (step1 > 0)
                    mainsprite.RotationalChange += -step2 * procent*(float)CohesionWeight;
                else
                    mainsprite.RotationalChange += step2 * procent * (float)CohesionWeight;

            }
        }
    }
    public void Seperate() {
    }
    public bool IsInRange() {

        return true;
    }
    public void GetSliderValue() {//needs to be called every game loop. should be changed to an event-handler so it is called when the slider changes.
        Slider slider1 = GetNode<HSlider>("Alignweight/HSliderAlignWeight");
        AlignWeight = slider1.Value;
        Slider slider2 = GetNode<HSlider>("Alignweight/HSliderCohesionWeight");
        CohesionWeight = slider2.Value;
    }

    public void Test() {
        QueueRedraw();

        Sprites[0].Sprite.Texture = (Texture2D)GD.Load("res://bluearrow.png");
        GD.Print(Sprites[0].Sprite.Rotation);
        GD.Print(Sprites[1].Sprite.Rotation);
        foreach (var sprite in Sprites) {
            if (sprite.Equals(Sprites[0])) continue;
            sprite.Sprite.Texture = (Texture2D)GD.Load("res://arrow.png");
        }
        foreach (var sprite in Sprites) {
            if (sprite.Equals(Sprites[0])) continue;
            if (Sprites[0].IsInRange(sprite))
                sprite.Sprite.Texture = (Texture2D)GD.Load("res://redarrow.png");
        }
        //Sprites[1].Sprite.Texture = (Texture2D)GD.Load("res://bluearrow.png");
    }
}




public class OwnSprite {
    public Sprite2D Sprite { get; }
    public Vector2 VelocityVEC { get; set; } // prefferbly dont use this
    public float VelocityNUM { get; set; }
    public float RotationalChange { get; set; }
    public OwnSprite() {
        Sprite = new Sprite2D();
        Sprite.Texture = (Texture2D)GD.Load("res://arrow.png");
        Sprite.Scale = new Vector2(0.075f, 0.075f);
        VelocityNUM = 1;
        VelocityVEC = new Vector2(0, 0);
        RotationalChange = 0;

        Random rnd = new Random();
        Sprite.Rotation = rnd.NextSingle() * Mathf.Pi * 2;
    }
    public bool IsInRange(OwnSprite other) {
        // all these values are squared
        float xdis = (other.Sprite.Position.X - Sprite.Position.X) * (other.Sprite.Position.X - Sprite.Position.X);
        float ydis = (other.Sprite.Position.Y - Sprite.Position.Y) * (other.Sprite.Position.Y - Sprite.Position.Y);
        float rdis = Overlord.Radius * Overlord.Radius;
        return (xdis + ydis - rdis) <= 0;
    }
    public bool IsInFOV(OwnSprite other) {
        //if (!IsInRange(other)) return false;
        float thisAngle = Sprite.Rotation;
        float otherAngle = MathF.PI - MathF.Acos((Sprite.Position - other.Sprite.Position).Normalized().X);

        float minAngle = thisAngle - MathF.PI / 3;
        if (minAngle < 0) minAngle += MathF.Tau;
        float maxAngle = thisAngle + MathF.PI / 3;
        if (maxAngle > MathF.Tau) maxAngle -= MathF.Tau;

        if (otherAngle < 0) otherAngle += MathF.Tau;
        if (otherAngle > MathF.Tau) otherAngle -= MathF.Tau;


        GD.Print("other: " + otherAngle);
        GD.Print("min: " + minAngle + " norm: " + thisAngle + " max: " + maxAngle);
        return (minAngle < otherAngle && maxAngle > otherAngle);
    }
}
