using Godot;
using System;

public partial class CompletedRunMenu : MarginContainer
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var diveAgainButton = GetNode<TextureButton>("SceneContainer/Parchment/ParchmentContainer/DiveAgainContainer/DiveAgain");
		diveAgainButton.Connect("pressed", new Callable(this, nameof(DiveAgainButtonPressed)));
		
		var inventoryButton = GetNode<TextureButton>("SceneContainer/Parchment/ParchmentContainer/ShopContainer/Shop");
		inventoryButton.Connect("pressed", new Callable(this, nameof(InventoryButtonPressed)));
		
		var shopButton = GetNode<TextureButton>("SceneContainer/Parchment/ParchmentContainer/ShopContainer/Shop");
		shopButton.Connect("pressed", new Callable(this, nameof(ShopButtonPressed)));
		
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

	private void DiveAgainButtonPressed()
	{
	}
	
	private void InventoryButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://Inventory.tscn");
	}

	private void ShopButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://Shop.tscn");
	}

}







