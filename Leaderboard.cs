using Godot;
using System;

public partial class Leaderboard : MarginContainer
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var backButton = GetNode<TextureButton>("SceneContainer/Parchment/BackArrowContainer/BackArrow");
		backButton.Connect("pressed", new Callable(this, nameof(BackArrowPressed)));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("go_back")) GetTree().ChangeSceneToFile("res://MainMenu.tscn");   
	}
	private void BackArrowPressed()
	{
		GetTree().ChangeSceneToFile("res://MainMenu.tscn");
	}
}



