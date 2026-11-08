using Godot;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody2D
{
	[Signal]
	public delegate void PlayerInteractsEventHandler();

	AnimatedSprite2D playerAnimation;
	private int speed;
	
	public enum inventoryContents
	{
		POINTS = 0,
		GOLD = 1,
		COMPASS = 2,
		CHALK = 3,
		TORCH_LVL = 4,
		KEY = 5,
		SPEED_POTION = 6,
		TRAP_DETECTOR = 7,
		TRAP_DISARMER = 8,
		SCAVENGE_TOOL = 9,
		DUNGEON_LVL = 10
	}

	public Dictionary<inventoryContents, int> inventory = new Dictionary<inventoryContents, int>{
		[inventoryContents.POINTS] = 0,
		[inventoryContents.GOLD] = 0,
		[inventoryContents.COMPASS] = 0,
		[inventoryContents.CHALK] = 0,
		[inventoryContents.TORCH_LVL] = 0,
		[inventoryContents.KEY] = 0,
		[inventoryContents.SPEED_POTION] = 0,
		[inventoryContents.TRAP_DETECTOR] = 0,
		[inventoryContents.TRAP_DISARMER] = 0,
		[inventoryContents.SCAVENGE_TOOL] = 0,
		[inventoryContents.DUNGEON_LVL] = 0
	};

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
