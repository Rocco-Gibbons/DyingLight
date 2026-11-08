using Godot;
using System;
using System.Security.Cryptography;

public partial class MainGame : Node2D
{
	[Export]
    public PackedScene GameScene { get; set; }

	MainMenu mainMenu;

	Leaderboard leaderboard;

	Shop shop;

	Inventory inventory;

	CompletedRunMenu completedRunMenu;

	Game newGame;

	bool gameLoaded;

	private void DeleteGame()
	{
		foreach (Room r in newGame.roomList)
		{
			r.roomItems.QueueFree();
			r.QueueFree();
		}
		gameLoaded = false;
	}

	private void RestartGame()
	{
		gameLoaded = true;
		
		completedRunMenu.Visible = false;
		completedRunMenu.SetProcess(false);
		newGame.MoveToFront();
		newGame.Show();

		newGame.StartGame();
	}

	private void StartNewGame()
	{
		gameLoaded = true;

		newGame = GameScene.Instantiate<Game>();
		AddChild(newGame);
		mainMenu.Visible = false;
		mainMenu.SetProcess(false);
		newGame.MoveToFront();
		newGame.Show();
		newGame.SetProcess(true);

		newGame.GoToMainMenu += ShowMainMenu;
		newGame.GoToRunMenu += ShowRunMenu;

		newGame.StartGame();
	}

	private void ShowMainMenu()
	{
		if (newGame != null)
		{
			DeleteGame();
			newGame.QueueFree();
			newGame = null;
		}
		leaderboard.Visible = false;
		leaderboard.SetProcess(false);
		mainMenu.Visible = true;
		mainMenu.SetProcess(true);
		mainMenu.MoveToFront();
	}

	private void ShowLeaderboard()
	{
		mainMenu.Visible = false;
		mainMenu.SetProcess(false);
		leaderboard.Visible = true;
		leaderboard.SetProcess(true);
		leaderboard.MoveToFront();
	}

	private void ShowRunMenu()
	{
		if (gameLoaded && newGame != null)
		{
			DeleteGame();
		}
		shop.Visible = false;
		shop.SetProcess(false);
		inventory.Visible = false;
		inventory.SetProcess(false);
		completedRunMenu.Visible = true;
		completedRunMenu.SetProcess(true);
		completedRunMenu.MoveToFront();
	}

	private void ShowShop()
	{
		shop.Visible = true;
		shop.SetProcess(true);
		completedRunMenu.Visible = false;
		completedRunMenu.SetProcess(false);
		shop.MoveToFront();
	}

	private void ShowInventory()
	{
		inventory.Visible = true;
		inventory.SetProcess(true);
		completedRunMenu.Visible = false;
		completedRunMenu.SetProcess(false);
		inventory.MoveToFront();
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		mainMenu = GetNode<MainMenu>("MainMenu");
		mainMenu.GoToLeaderboard += ShowLeaderboard;
		mainMenu.StartNewGame += StartNewGame;
		leaderboard = GetNode<Leaderboard>("Leaderboard");
		leaderboard.GoToMainMenu += ShowMainMenu;
		leaderboard.SetProcess(false);
		shop = GetNode<Shop>("Shop");
		shop.GoToRunMenu += ShowRunMenu;
		shop.SetProcess(false);
		inventory = GetNode<Inventory>("Inventory");
		inventory.GoToRunMenu += ShowRunMenu;
		inventory.SetProcess(false);
		completedRunMenu = GetNode<CompletedRunMenu>("CompletedRunMenu");
		completedRunMenu.GoToShop += ShowShop;
		completedRunMenu.GoToInventory += ShowInventory;
		completedRunMenu.RestartGame += RestartGame;
		completedRunMenu.SetProcess(false);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
