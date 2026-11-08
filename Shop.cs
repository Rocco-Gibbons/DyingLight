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
	private void CompassInfoButtonPressed()
	{
		var container = GetNode<TextureRect>("SceneContainer/InfoContainer/CompassInfo");
		container.Show();
	}
	private void CompassCloseButtonPressed()
	{
		var container = GetNode<TextureRect>("SceneContainer/InfoContainer/CompassInfo");
		container.Hide();
	}	
	private void PurchaseCompass()
	{
	}
	

	private void ChalkInfoButtonPressed()
	{
		var container = GetNode<TextureRect>("SceneContainer/InfoContainer/ChalkInfo");
		container.Show();
	}
	private void ChalkCloseButtonPressed()
	{
		var container = GetNode<TextureRect>("SceneContainer/InfoContainer/ChalkInfo");
		container.Hide();
	}
	private void PurchaseChalk()
	{
	}


	private void TorchInfoButtonPressed()
	{
		var container = GetNode<TextureRect>("SceneContainer/InfoContainer/TorchInfo");
		container.Show();
	}
	private void TorchCloseButtonPressed()
	{
		var container = GetNode<TextureRect>("SceneContainer/InfoContainer/TorchInfo");
		container.Hide();
	}
	private void PurchaseTorchUpgrade()
	{
	}


	private void KeyInfoButtonPressed()
	{
		var container = GetNode<TextureRect>("SceneContainer/InfoContainer/KeyInfo");
		container.Show();
	}
	private void KeyCloseButtonPressed()
	{
		var container = GetNode<TextureRect>("SceneContainer/InfoContainer/KeyInfo");
		container.Hide();
	}
	private void PurchaseKey()
	{
	}


	private void SpeedInfoButtonPressed()
	{
		var container = GetNode<TextureRect>("SceneContainer/InfoContainer/SpeedInfo");
		container.Show();
	}
	private void SpeedCloseButtonPressed()
	{
		var container = GetNode<TextureRect>("SceneContainer/InfoContainer/SpeedInfo");
		container.Hide();
	}
	private void PurchaseSpeedPotion()
	{
	}


	private void DetectInfoButtonPressed()
	{
		var container = GetNode<TextureRect>("SceneContainer/InfoContainer/TrapDetectorInfo");
		container.Show();
	}
	private void DetectorCloseButtonPressed()
	{
		var container = GetNode<TextureRect>("SceneContainer/InfoContainer/TrapDetectorInfo");
		container.Hide();
	}
	private void PurchaseTrapDetector()
	{
	}


	private void DisarmInfoButtonPressed()
	{
		var container = GetNode<TextureRect>("SceneContainer/InfoContainer/TrapDisarmerInfo");
		container.Show();
	}
	private void DisarmCloseButtonPressed()
	{
		var container = GetNode<TextureRect>("SceneContainer/InfoContainer/TrapDisarmerInfo");
		container.Hide();
	}
	private void PurchaseTrapDisarmer()
	{
	}


	private void ScavengeInfoButtonPressed()
	{
		var container = GetNode<TextureRect>("SceneContainer/InfoContainer/ScavengeInfo");
		container.Show();
	}
	private void ScavengeCloseButtonPressed()
	{
		var container = GetNode<TextureRect>("SceneContainer/InfoContainer/ScavengeInfo");
		container.Hide();
	}
	private void PurchaseScavengeTool()
	{
	}


	private void ImproveDungeonInfoButtonPressed()
	{
		var container = GetNode<TextureRect>("SceneContainer/InfoContainer/ImproveDungeonInfo");
		container.Show();
	}
	private void ImproveDungeonCloseButtonPressed()
	{
		var container = GetNode<TextureRect>("SceneContainer/InfoContainer/ImproveDungeonInfo");
		container.Hide();
	}
	private void PurchaseDungeonUpgrade()
	{
	}

}




























