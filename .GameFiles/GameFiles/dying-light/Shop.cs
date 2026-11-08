using Godot;
using System;

public partial class Character {
	public int gold = 100;	
	public int compass = 0;
	public int chalk = 0;
	public int torchLvl = 0;
	public int key = 0;
	public int speedPotion = 0;
	public int detector = 0;
	public int disarmer = 0;
	public int scavenge = 0;
	public int dungeonLvl = 0;
}

public partial class Shop : MarginContainer
{
	[Signal]
	public delegate void GoToRunMenuEventHandler();

	private void GoBackFromShop()
	{
		EmitSignal(SignalName.GoToRunMenu);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Character john = new Character();
		var gold_label = GetNode<RichTextLabel>("SceneContainer/Parchment/ParchmentContainer/GoldCounter");
		gold_label.BbcodeEnabled = true;
		gold_label.Clear();
		gold_label.Text = $"[center][color=black]${john.gold}  [/color][/center]";
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("go_back")) GoBackFromShop();      
	}
	
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
		/*
			Logic:
				Check if you already have the compass
				If yes: Cannot buy again
				
				Check if you have enough money
				If no: Cannot buy
				
				Take cost away from total amount of money
				Add compass to character		
		*/
		int cost = 100;
		
		// John is a placeholder character to represent logic.
		Character john = new Character();
		
		if (john.compass >= 1) return;
		if (john.gold < cost) return;
		
		john.compass += 1;
		john.gold -= cost;
		GD.Print($"Bought Compass, {john.compass} {john.gold}");
		
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
		/*
			Logic:
				Check if you have enough money
				If no: Cannot buy
				
				Take cost away from total amount of money
				Add another chalk to character
		*/
		int cost = 50;

		// John is a placeholder character to represent logic.
		Character john = new Character();
		
		if (john.gold < cost) return;
		
		john.chalk += 1;
		john.gold -= cost;
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
		/*
			Logic:
				Check level of current torch
				
				Check if you have enough money for next level
				If no: Cannot buy
				
				Take cost away from total amount of money
				Upgrade torch level		
		*/
		int[] cost = {30, 75, 150};

		// John is a placeholder character to represent logic.
		Character john = new Character();
		int torchLvl = john.torchLvl;
		
		if (john.torchLvl >= 2) return;
		if (john.gold < cost[torchLvl]) return;
		
		john.torchLvl += 1;
		john.gold -= cost[torchLvl];
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
		/*
			Logic:
				Check if you have enough money
				If no: Cannot buy
				
				Take cost away from total amount of money
				Add another key to character	
		*/
		int cost = 50;

		// John is a placeholder character to represent logic.
		Character john = new Character();
		
		if (john.gold < cost) return;
		
		john.key += 1;
		john.gold -= cost;
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
		/*
			Logic:
				Check if you have enough money 
				If no: Cannot buy
				
				Take cost away from total amount of money
				Add another speed potion to character		
		*/
		int cost = 75;

		// John is a placeholder character to represent logic.
		Character john = new Character();
		
		if (john.gold < cost) return;
		
		john.speedPotion += 1;
		john.gold -= cost;
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
		/*
			Logic:
				Check if you already have the trap detector
				If yes: Cannot buy again
				
				Check if you have enough money
				If no: Cannot buy
				
				Take cost away from total amount of money
				Add trap detector to character		
		*/
		int cost = 100;
		
		// John is a placeholder character to represent logic.
		Character john = new Character();
		
		if (john.detector >= 1) return;
		if (john.gold < cost) return;
		
		john.detector += 1;
		john.gold -= cost;
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
		/*
			Logic:
				Check if you already have the trap disarmer
				If yes: Cannot buy again
				
				Check if you have enough money
				If no: Cannot buy
				
				Take cost away from total amount of money
				Add trap disarmer to character		
		*/
		int cost = 100;
		
		// John is a placeholder character to represent logic.
		Character john = new Character();
		
		if (john.disarmer >= 1) return;
		if (john.gold < cost) return;
		
		john.disarmer += 1;
		john.gold -= cost;
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
		/*
			Logic:
				Check if you already have the scavenge tool
				If yes: Cannot buy again
				
				Check if you have enough money
				If no: Cannot buy
				
				Take cost away from total amount of money
				Add scavenge tool to character		
		*/
		int cost = 75;
		
		// John is a placeholder character to represent logic.
		Character john = new Character();
		
		if (john.scavenge >= 1) return;
		if (john.gold < cost) return;
		
		john.scavenge += 1;
		john.gold -= cost;
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
		/*
			Logic:
				Check level of current dungeon
				
				Check if you have enough money for next level
				If no: Cannot buy
				
				Take cost away from total amount of money
				Upgrade dungeon level		
		*/
		int cost = 200;
		
		// John is a placeholder character to represent logic.
		Character john = new Character();
		
		if (john.gold < cost) return;
		
		john.dungeonLvl += 1;
		john.gold -= cost;
	}
}





























