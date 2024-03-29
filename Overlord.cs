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
    }
    public void MoveSprites() {
        foreach (var sprite in Sprites) {
            Vector2 newOffset = new Vector2(sprite.VelocityNUM * MathF.Cos(sprite.Sprite.Rotation), sprite.VelocityNUM * MathF.Sin(sprite.Sprite.Rotation));

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
    public void Alignment() {//find hvilke boids der er i radius. if is in range then.  gem deres alignment/retning i xy og antal. bagefter divider med x og y med antal. 

        float radius = 100;
        for (int i = 0; i < Sprites.Count; i++) {
            float newalignment;
            float xLowRange;
            float xHighRange;
            float yLowRange;
            float yHighRange;

            xLowRange = Sprites[0].Sprite.Position.X - radius;
            xHighRange = Sprites[0].Sprite.Position.X - radius;
            yLowRange = Sprites[0].Sprite.Position.Y - radius;
            yHighRange = Sprites[0].Sprite.Position.Y - radius;

            for (int j = 0; j < Sprites.Count; j++) {

            }
            if (xLowRange < 0) { }

            //Sprites[0].Position = Sprites[1].Position;

        }
    }
    public bool IsInRange() {

        return true;
    }
    public void CalcAlignment() {

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

}
