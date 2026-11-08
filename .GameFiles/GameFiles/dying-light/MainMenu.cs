using Godot;
using System;

public partial class MainMenu : MarginContainer
{
	[Signal]
	public delegate void GoToLeaderboardEventHandler();

	[Signal]
	public delegate void StartNewGameEventHandler();

	private void NewGameButtonPressed()
	{
		EmitSignal(SignalName.StartNewGame);
	}

	private void LeaderboardButtonPressed()
	{
		EmitSignal(SignalName.GoToLeaderboard);
	}
	
	private void QuitButtonPressed()
	{
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
