using Godot;
using System;
using System.Collections.Generic;

public partial class Overlord : Node2D {
    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        Sprites = new List<Sprite2D>();

        for (int i = 0; i < 100; i++) {
            Sprite2D sprite2D = new Sprite2D();
            sprite2D.Texture = (Texture2D)GD.Load("res://icon.svg");
            //base._Ready();
            Node2D node = GetNode<Node2D>(".");
            Random random = new Random();
            sprite2D.Position = new Vector2(random.NextSingle() * 1000, random.NextSingle() * 500);

            Sprites.Add(sprite2D);
            node.AddChild(sprite2D);

        }

    }
    public List<Sprite2D> Sprites { get; set; }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
    }
    public void Alignment() {//find hvilke boids der er i radius. if is in range then. så gem deres alignment/retning i xy og antal. bagefter divider med x og y med antal. 

        float radius = 100;
        for (int i = 0; i < Sprites.Count; i++) {
            float newalignment;
            float xLowRange;
            float xHighRange;
            float yLowRange;
            float yHighRange;

            xLowRange = Sprites[0].Position.X - radius;
            xHighRange = Sprites[0].Position.X - radius;
            yLowRange = Sprites[0].Position.Y - radius;
            yHighRange = Sprites[0].Position.Y - radius;

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
