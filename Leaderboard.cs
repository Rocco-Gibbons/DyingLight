using Godot;
using System;

public class Leaderboard : MarginContainer
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var backButton = GetNode<TextureButton>("SceneContainer/Parchment/BackArrowContainer/BackArrow");
		backButton.Connect("pressed", this, nameof(BackArrowPressed));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("go_back")) GetTree().ChangeScene("res://MainMenu.tscn");   
	}
	private void BackArrowPressed()
	{
		GetTree().ChangeScene("res://MainMenu.tscn");
	}
}



