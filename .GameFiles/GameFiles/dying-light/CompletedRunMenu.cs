using Godot;
using System;

public partial class CompletedRunMenu : MarginContainer
{
	[Signal] 
	public delegate void GoToShopEventHandler();
	[Signal] 
	public delegate void GoToInventoryEventHandler();
	[Signal]
	public delegate void RestartGameEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	private void DiveAgainButtonPressed()
	{
		EmitSignal(SignalName.RestartGame);
	}
	
	private void InventoryButtonPressed()
	{
		EmitSignal(SignalName.GoToInventory);
	}

	private void ShopButtonPressed()
	{
		EmitSignal(SignalName.GoToShop);
	}

}







