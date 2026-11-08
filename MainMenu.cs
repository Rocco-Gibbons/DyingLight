using Godot;
using System;

public partial class MainMenu : MarginContainer
{
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var leaderboardButton = GetNode<TextureButton>("SceneContainer/LeftSplitContainer/TextContainer/InnerTextContainer/ButtonContainer/ButtonListContainer/LeaderboardContainer/Leaderboard");
		leaderboardButton.Connect("pressed", new Callable(this, nameof(LeaderboardButtonPressed)));

	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  	//public override void _Process(float delta)
  	//{

  	//}

	private void LeaderboardButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://Leaderboard.tscn");
	}
	
	private void QuitButtonPressed()
	{
		GetTree().Quit();
	}

}





