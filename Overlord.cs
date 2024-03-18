using Godot;
using System;
using System.Collections.Generic;

public partial class Overlord : Node2D {
    [Export]
    public int SpriteCount = 10;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        Sprites = new List<Sprite2D>();

        for (int i = 0; i < SpriteCount; i++) {
            Sprite2D sprite2D = new Sprite2D();
            sprite2D.Texture = (Texture2D)GD.Load("res://icon.svg");
            //base._Ready();
            Node2D node = GetNode<Node2D>(".");
            Random random = new Random();
            sprite2D.Position = new Vector2(random.NextSingle() * 1000, random.NextSingle() * 500);

            RigidBody2D rigidBody = new RigidBody2D();
            rigidBody.AngularVelocity = 1;
            rigidBody.Rotation = random.NextSingle() * 2;
            rigidBody.GravityScale = 0;

            sprite2D.AddChild(rigidBody);
            

            Sprites.Add(sprite2D);
            node.AddChild(sprite2D);


        }

    }
    public List<Sprite2D> Sprites { get; set; }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
        
    }
}
