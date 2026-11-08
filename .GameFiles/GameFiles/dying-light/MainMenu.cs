using Godot;
using System;

public partial class MainMenu : MarginContainer
{
	[Signal]
	public delegate void GoToLeaderboardEventHandler();

	[Signal]
	public delegate void StartNewGameEventHandler();
	
	//var player = new AudioStreamPlayer();
	//AddChild(player);
	//player.Stream = GD.Load<AudioStream>("res://assets/sfx/select.mp3");
	
	private void NewGameButtonPressed()
	{
		var player = new AudioStreamPlayer();
		AddChild(player);
		player.Stream = GD.Load<AudioStream>("res://assets/sfx/select.mp3");
		player.Play();
		EmitSignal(SignalName.StartNewGame);
	}

	private void LeaderboardButtonPressed()
	{
		var player = new AudioStreamPlayer();
		AddChild(player);
		player.Stream = GD.Load<AudioStream>("res://assets/sfx/select.mp3");
		player.Play();
		EmitSignal(SignalName.GoToLeaderboard);
	}
	
	private async void QuitButtonPressed()
	{
		var player = new AudioStreamPlayer();
		AddChild(player);
		player.Stream = GD.Load<AudioStream>("res://assets/sfx/select.mp3");
		player.Play();
		await ToSignal(GetTree().CreateTimer(0.2), SceneTreeTimer.SignalName.Timeout);
		GetTree().Quit();
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

 // Called every frame. 'delta' is the elapsed time since the previous frame.
  	public override void _Process(double delta)
  	{

  	}
}
