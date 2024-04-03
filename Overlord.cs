using Godot;
using System;
using System.Collections.Generic;

public partial class Overlord : Node2D {
    [Export]
    static public int SpriteCount = 100;
    [Export]
    static public bool EndlessScreen = true;

    static public float Radius = 250;

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

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
        MoveSprites();
        RotationNormalizer();
        //Alignment();
        Test();
    }
    public void MoveSprites() {
        foreach (var sprite in Sprites) {
            Vector2 newOffset = new(sprite.VelocityNUM * MathF.Cos(sprite.Sprite.Rotation), sprite.VelocityNUM * MathF.Sin(sprite.Sprite.Rotation));

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
                sprite.Sprite.Rotation %= Mathf.Tau;
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

            /*if (average>=0) {
                mainsprite.Sprite.Rotate((average + mainsprite.Sprite.Rotation) * procent);
            }
            else {
                mainsprite.Sprite.Rotate((average - mainsprite.Sprite.Rotation) * procent);
            }*/
            mainsprite.Sprite.Rotate((average - mainsprite.Sprite.Rotation) * procent);

            //mainsprite.Sprite.Rotate(mainsprite.Sprite.Rotation - average * procent);
            //mainsprite.Sprite.Rotate((average + mainsprite.Sprite.Rotation) * procent);


            //float tem =temp - mainsprite.Sprite.Rotation;
            //mainsprite.Sprite.Rotation = cummrotation / amount;
            if (mainsprite.Equals(Sprites[0])) {
                GD.Print(average);
                GD.Print(cummrotation);
                GD.Print(mainsprite.Sprite.Rotation);
            }
            //mainsprite.Sprite.Rotate(average*procent);
        }

    }
    public void Seperate() {
        foreach (var mainsprite in Sprites) {

        }
    }
    public bool IsInRange() {

        return true;
    }
    public void CalcAlignment() {

    }
    public void Test() {
        Sprites[0].Sprite.Texture = (Texture2D)GD.Load("res://bluearrow.png");
        GD.Print(Sprites[0].Sprite.Rotation);
        GD.Print(Sprites[1].Sprite.Rotation);
        foreach (var sprite in Sprites) {
            if (sprite.Equals(Sprites[0])) continue;
            sprite.Sprite.Texture = (Texture2D)GD.Load("res://arrow.png");
        }
        foreach (var sprite in Sprites) {
            if (sprite.Equals(Sprites[0])) continue;
            if (Sprites[0].IsInFOV(sprite))
                sprite.Sprite.Texture = (Texture2D)GD.Load("res://redarrow.png");
        }
        Sprites[1].Sprite.Texture = (Texture2D)GD.Load("res://bluearrow.png");
    }
}




public class OwnSprite {
    public Sprite2D Sprite { get; }
    public Vector2 VelocityVEC { get; set; } // prefferbly dont use this
    public float VelocityNUM { get; set; }
    public OwnSprite() {
        Sprite = new Sprite2D();
        Sprite.Texture = (Texture2D)GD.Load("res://arrow.png");
        Sprite.Scale = new Vector2(0.075f, 0.075f);
        VelocityNUM = 1;
        VelocityVEC = new Vector2(0, 0);

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
        float otherAngle = other.Sprite.Rotation;
        float minAngle = thisAngle - MathF.PI / 2;
        if (minAngle < 0) minAngle += MathF.Tau;
        float maxAngle = thisAngle + MathF.PI / 2;
        if (maxAngle > MathF.Tau) maxAngle -= MathF.Tau;

        return (minAngle < otherAngle && maxAngle > otherAngle);
    }
}
