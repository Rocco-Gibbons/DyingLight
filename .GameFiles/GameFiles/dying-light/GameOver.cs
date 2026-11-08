using Godot;
using System;

public partial class GameOverClass : MarginContainer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var pointsLabel = GetNode<RichTextLabel>("Background/SceneContainer/PointsLabelContainer/PointsLabel");
		pointsLabel.BbcodeEnabled = true;
		pointsLabel.Clear();
		
		// This is for you to do
		//pointsLabel.Text = $"[center][color=black]Total Score: ${player.inventory[gold]}[/color][/center]";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
