using Godot;
using System;

public partial class Leaderboard : MarginContainer
{
	[Signal]
	public delegate void GoToMainMenuEventHandler();

	private void GoBackFromLeaderboard()
	{
		var player = new AudioStreamPlayer();
		AddChild(player);
		player.Stream = GD.Load<AudioStream>("res://assets/sfx/select.mp3");
		player.Play();
		EmitSignal(SignalName.GoToMainMenu);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("go_back")) GoBackFromLeaderboard();   
	}
}
