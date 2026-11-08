using Godot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

public partial class Main : Node2D
{
	[Export]
    public PackedScene RoomScene { get; set; }

	Random rn = new Random();

	Player player;

	bool gameStarted = false;

	Button startButton;
	ColorRect overlay;

	int currentRoomIndex;
	List<Room> roomList = [];
	List<int[]> roomTree = []; //for storing connections between rooms
	//roomTree structure:
	//0: index of room above (or -1 if none)
	//1: index of room below (or -1 if none)
	//2: index of room to the left (or -1 if none)
	//3: index of room to the right (or -1 if none)
	List<List<int>> roomRepresentation = [[-1]];
	List<(int, int, Vector2I, Vector2I)> roomChanges = []; //index to change to, area1, area2

	private void ExpandRepresentation(Room.Dir dir)
	{
		switch (dir)
		{
			case Room.Dir.LEFT:
				foreach (List<int> l in roomRepresentation)
				{
					l.Insert(0, -1);
				}
				break;
			case Room.Dir.RIGHT:
				foreach (List<int> l in roomRepresentation)
				{
					l.Add(-1);
				}
				break;
			case Room.Dir.UP:
				List<int> newTopLayer = [];
				for (int i = 0; i < roomRepresentation[0].Count; i++)
				{
					newTopLayer.Add(-1);
				}
				roomRepresentation.Insert(0, newTopLayer);
				break;
			case Room.Dir.DOWN:
				List<int> newBottomLayer = [];
				for (int i = 0; i < roomRepresentation[0].Count; i++)
				{
					newBottomLayer.Add(-1);
				}
				roomRepresentation.Add(newBottomLayer);
				break;
		}
	}

	private Vector2I GetPlayerPos()
	{
		Vector2 playerPos = player.GlobalPosition;
		Vector2 gridPos = roomList[currentRoomIndex].ToLocal(playerPos);
		return roomList[currentRoomIndex].roomLayout.LocalToMap(gridPos);
	}

	private int CheckIfChangeRoom()
	{
		Vector2I playerPos = GetPlayerPos();
		foreach ((int from, int to, Vector2I pos1, Vector2I pos2) in roomChanges)
		{
			if (from == currentRoomIndex && (playerPos == pos1 || playerPos == pos2))
			{
				return to;
			}
		}
		return -1;
	}

	private Room.Dir GetOppositeDir(Room.Dir dir)
	{
		switch (dir)
		{
			case Room.Dir.DOWN:
				return Room.Dir.UP;
			case Room.Dir.UP:
				return Room.Dir.DOWN;
			case Room.Dir.RIGHT:
				return Room.Dir.LEFT;
			default:
				return Room.Dir.RIGHT;
		}
	}

	private void CreateRoomChanges()
	{
		int[] roomLeaf;
		int xStart, xSize, yStart, ySize;
		for (int i = 0; i < roomTree.Count; i++)
		{
			xStart = roomList[i].xStart + 1;
			xSize = roomList[i].xSize + 1;
			yStart = roomList[i].yStart + 1;
			ySize = roomList[i].ySize + 1;
			roomLeaf = roomTree[i];
			if (roomLeaf[0] != -1) //up
			{
				roomChanges.Add((i, roomLeaf[0], new Vector2I(xStart + (xSize / 2) - 1, yStart), new Vector2I(xStart + (xSize / 2), yStart)));
			}
			if (roomLeaf[1] != -1 && roomLeaf[1] != -100) //down, extra as start has this
			{
				roomChanges.Add((i, roomLeaf[1], new Vector2I(xStart + (xSize / 2) - 1, yStart + ySize - 1), new Vector2I(xStart + (xSize / 2), yStart + ySize - 1)));
			}
			if (roomLeaf[2] != -1) //left
			{
				roomChanges.Add((i, roomLeaf[2], new Vector2I(xStart, yStart + (ySize / 2) - 1), new Vector2I(xStart, yStart + (ySize / 2))));
			}
			if (roomLeaf[3] != -1) //right
			{
				roomChanges.Add((i, roomLeaf[3], new Vector2I(xStart + xSize - 1, yStart + (ySize / 2) - 1), new Vector2I(xStart + xSize - 1, yStart + (ySize / 2))));
			}
		}
	}

	private void ConnectRooms(int index1, int index2, Room.Dir dir) //dir is direction from first
	{
		switch (dir)
		{
			case Room.Dir.UP:
				roomTree[index1][0] = index2;
				roomTree[index2][1] = index1;
				break;
			case Room.Dir.DOWN:
				roomTree[index1][1] = index2;
				roomTree[index2][0] = index1;
				break;
			case Room.Dir.RIGHT:
				roomTree[index1][3] = index2;
				roomTree[index2][2] = index1;
				break;
			default:
				roomTree[index1][2] = index2;
				roomTree[index2][3] = index1;
				break;
		}
	}

	private void MakeRoom(int connectIndex) //index 0 is always start room
	{
		Room newRoom = RoomScene.Instantiate<Room>();
		roomList.Add(newRoom);
		if (connectIndex == -1)
		{
			newRoom.connectedRooms = [Room.Dir.DOWN];
			newRoom.isStart = true;
			roomTree.Add([-1, -100, -1, -1]);
		}
		else
		{
			Room.Dir newConnection;
			do
			{
				newConnection = (Room.Dir)rn.Next(0,4);
			} while(roomTree[connectIndex][(int)newConnection] != -1);
			newRoom.connectedRooms.Add(GetOppositeDir(newConnection));
			newRoom.isStart = false;
			roomList[connectIndex].connectedRooms.Add(newConnection);
			roomTree.Add([-1, -1, -1, -1]);
			ConnectRooms(roomList.Count - 1, connectIndex, GetOppositeDir(newConnection));
		}
	}

	private void InitAllRooms() //happens after so all connections can be generated first
	{
		foreach (Room r in roomList)
		{
			AddChild(r);
		}
	}

	private void CreateDungeon(int rooms)
	{
		int roomIndex;
		int[] roomLeaf;
		//first make start room:
		MakeRoom(-1); //-1 signals no connections as it is the first one
		rooms--;
		for (int i = 0; i < rooms; i++)
		{
			//pick a random room with a free connection
			do
			{
				roomIndex = rn.Next(0,roomList.Count);
				roomLeaf = roomTree[roomIndex];
			} while (!roomLeaf.Contains(-1));
			//create a new room the connects to that one
			MakeRoom(roomIndex);			
		}

		InitAllRooms();
		CreateRoomChanges();
	}

	async private void StartButtonPressed()
	{
		startButton = GetNode<Button>("StartButton");
		startButton.Hide();

		CreateDungeon(100);

		currentRoomIndex = 0;
		roomList[0].PlayerEntered();

		gameStarted = true;

		CharacterBody2D player = GetNode<CharacterBody2D>("Player");
		player.MoveToFront();

		overlay.Show();
		overlay.MoveToFront();
		overlay.Color = new Color(0, 0, 0, 1);
		float overlayOpacity = 1;
		while (overlayOpacity != 0)
		{
			overlayOpacity -= (float)0.01;
			overlay.Color = new Color(0, 0, 0, overlayOpacity);
			await ToSignal(GetTree().CreateTimer(0.01), SceneTreeTimer.SignalName.Timeout);
		}
	}

	private void OnPlayerInteract()
	{
		Vector2I playerPos = GetPlayerPos();
		int xStart, xSize, yStart, ySize, x, y;
		foreach ((int xChange, int yChange) in new List<(int, int)>{(0, 1), (1, 0), (0, -1), (-1, 0), (1, 1), (1, -1), (-1, 1), (-1, -1)})
		{
			xStart = roomList[currentRoomIndex].xStart;
			xSize = roomList[currentRoomIndex].xSize;
			yStart = roomList[currentRoomIndex].yStart;
			ySize = roomList[currentRoomIndex].ySize;
			x = playerPos.X + xChange;
			y = playerPos.Y + yChange;
			if (currentRoomIndex == 0 && (x == xStart + (xSize / 2) || x == xStart + (xSize / 2) + 1) && y == yStart + ySize - 2)
			{
				GameOver(true);
			}
			else if (x - (xStart + 2) >= 0 && x - (xStart + 2) < xSize - 2 && y - (yStart + 2) >= 0 && y - (yStart + 2) < ySize - 2)
			{
				x -= xStart + 2;
				y -= yStart + 2;
				switch (roomList[currentRoomIndex].roomItems.spriteMap[x, y])
				{
					case Items.Sprites.SMALL_COBWEB:
						break;
					case Items.Sprites.BIG_COBWEB:
						break;
					case Items.Sprites.SMALL_BOX:
						break;
					case Items.Sprites.BIG_BOX:
						break;
					case Items.Sprites.SMALL_CHEST:
						break;
					case Items.Sprites.BIG_CHEST:
						break;
					case Items.Sprites.BONES:
						break;
					case Items.Sprites.SKULL_AND_BONES:
						break;
					case Items.Sprites.KEY:
						break;
				}
			}
		}
	}

	private void GameOver(bool succeeded)
	{
		if (succeeded)
		{
			GD.Print("Yay!");
		}
		else
		{
			GD.Print("BooHoo");
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetNode<Player>("Player");
		player.PlayerInteracts += OnPlayerInteract;

		overlay = GetNode<ColorRect>("Overlay");
		overlay.Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (gameStarted)
		{
			int newIndex = CheckIfChangeRoom();
			if (newIndex != -1)
			{
				player.Position = new Vector2(144, 112); //centre of screen
				roomList[currentRoomIndex].PlayerLeft();
				currentRoomIndex = newIndex;
				roomList[currentRoomIndex].PlayerEntered();
			}
		}
	}
}
