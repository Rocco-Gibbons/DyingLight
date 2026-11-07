using Godot;
using System;

public class Shop : MarginContainer
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var gold_label = GetNode<RichTextLabel>("SceneContainer/Parchment/ParchmentContainer/GoldCounter");
		gold_label.BbcodeEnabled = true;
		gold_label.Clear();
		gold_label.BbcodeText = "[right][color=black]$money  [/color][/right]";
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
