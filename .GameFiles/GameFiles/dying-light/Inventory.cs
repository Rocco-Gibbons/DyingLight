using Godot;
using System;

public partial class Inventory : MarginContainer
{
	[Signal]
	public delegate void GoToRunMenuEventHandler();

	private void GoBackFromInventory()
	{
		EmitSignal(SignalName.GoToRunMenu);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("go_back")) GoBackFromInventory();      
	}
}


