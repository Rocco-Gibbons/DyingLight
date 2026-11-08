using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Signal]
	public delegate void PlayerInteractsEventHandler();

	AnimatedSprite2D playerAnimation;
	private int speed;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		speed = 100;
		playerAnimation = GetNode<AnimatedSprite2D>("PlayerAnimation");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("interact")) EmitSignal(SignalName.PlayerInteracts);

		var velocity = Vector2.Zero;

		if (Input.IsActionPressed("mv_right")) velocity.X += 1;

		if (Input.IsActionPressed("mv_left")) velocity.X -= 1;

		if (Input.IsActionPressed("mv_down")) velocity.Y += 1;

		if (Input.IsActionPressed("mv_up")) velocity.Y -= 1;

		Velocity = velocity.Normalized() * speed;

		if (velocity.Y > 0)
		{
			playerAnimation.Animation = "down";
		}
		else if (velocity.Y < 0)
		{
			playerAnimation.Animation = "up";
		}
		else if (velocity.X > 0)
		{
			playerAnimation.Animation = "right";
		}
		else if (velocity.X < 0)
		{
			playerAnimation.Animation = "left";
		}

		if (velocity.Length() > 0)
		{
			playerAnimation.Play();
		}
		else
		{
			playerAnimation.Stop();
		}

		MoveAndSlide();
	}
}
