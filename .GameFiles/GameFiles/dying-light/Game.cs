using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

public partial class Game : Node2D
{
	[Signal]
	public delegate void GoToMainMenuEventHandler();

	[Signal]
	public delegate void GoToRunMenuEventHandler();

	[Export]
    public PackedScene RoomScene { get; set; }

	Stopwatch gameTimer;

	Random rn = new Random();

	Player player;

	bool gameStarted = false;

	Button startButton;
	ColorRect overlay;

	int currentRoomIndex;
	public List<Room> roomList;
	List<int[]> roomTree; //for storing connections between rooms
	//roomTree structure:
	//0: index of room above (or -1 if none)
	//1: index of room below (or -1 if none)
	//2: index of room to the left (or -1 if none)
	//3: index of room to the right (or -1 if none)
	List<List<int>> roomRepresentation;
	List<(int, int, Vector2I, Vector2I)> roomChanges; //index to change to, area1, area2

	private void ResetGameElements()
	{
		roomList = [];
		roomTree = [];
		roomRepresentation = [[-1, -1, -1], [-1, 0, -1], [-1, -100, -1]];
		roomChanges = [];
		player.Position = new Vector2(144, 112);
	}

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

	private List<Room.Dir> CanPlaceRoom(int roomIndex)
	{
		List<Room.Dir> toReturn = [];
		for (int a = 0; a < roomRepresentation.Count; a++)
		{
			for (int b = 0; b < roomRepresentation[0].Count; b++)
			{
				if (roomRepresentation[a][b] == roomIndex)
				{
					if (a == roomRepresentation.Count - 1)
					{
						ExpandRepresentation(Room.Dir.DOWN);
						return CanPlaceRoom(roomIndex);
					}
					else if (b == roomRepresentation[0].Count - 1)
					{
						ExpandRepresentation(Room.Dir.RIGHT);
						return CanPlaceRoom(roomIndex);
					}
					else if (a == 0)
					{
						ExpandRepresentation(Room.Dir.UP);
						return CanPlaceRoom(roomIndex);
					}
					else if (b == 0)
					{
						ExpandRepresentation(Room.Dir.LEFT);
						return CanPlaceRoom(roomIndex);
					}
					else
					{
						if (roomRepresentation[a + 1][b] == -1) toReturn.Add(Room.Dir.DOWN);
						if (roomRepresentation[a][b + 1] == -1) toReturn.Add(Room.Dir.RIGHT);
						if (roomRepresentation[a - 1][b] == -1) toReturn.Add(Room.Dir.UP);
						if (roomRepresentation[a][b - 1] == -1) toReturn.Add(Room.Dir.LEFT);
					}
				}
			}
		}
		return toReturn;
	}

	private void AddRepresentation(int fromIndex, int newIndex, Room.Dir dir)
	{
		int x = 0, y = 0;
		for (int i = 0; i < roomRepresentation.Count; i++)
		{
			for (int j = 0; j < roomRepresentation[0].Count; j++)
			{
				if (roomRepresentation[i][j] == fromIndex)
				{
					x = i;
					y = j;
				}
			}
		}

		switch (dir)
		{
			case Room.Dir.UP:
				roomRepresentation[x - 1][y] = newIndex;
				break;
			case Room.Dir.DOWN:
				roomRepresentation[x + 1][y] = newIndex;
				break;
			case Room.Dir.LEFT:
				roomRepresentation[x][y - 1] = newIndex;
				break;
			case Room.Dir.RIGHT:
				roomRepresentation[x][y + 1] = newIndex;
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
			List<Room.Dir> possibleConnections = CanPlaceRoom(connectIndex);
			Room.Dir newConnection;
			do
			{
				newConnection = (Room.Dir)rn.Next(0,4);
			} while(!possibleConnections.Contains(newConnection));
			newRoom.connectedRooms.Add(GetOppositeDir(newConnection));
			newRoom.isStart = false;
			roomList[connectIndex].connectedRooms.Add(newConnection);
			roomTree.Add([-1, -1, -1, -1]);
			ConnectRooms(roomList.Count - 1, connectIndex, GetOppositeDir(newConnection));

			AddRepresentation(connectIndex, roomList.Count - 1, newConnection);
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
		//first make start room:
		MakeRoom(-1); //-1 signals no connections as it is the first one
		rooms--;
		for (int i = 0; i < rooms; i++)
		{
			//pick a random room with a free connection
			do
			{
				roomIndex = rn.Next(0,roomList.Count);
			} while (CanPlaceRoom(roomIndex).Count == 0);
			//create a new room the connects to that one
			MakeRoom(roomIndex);			
		}

		InitAllRooms();
		CreateRoomChanges();
	}

	async public void StartGame()
	{
		ResetGameElements();

		gameTimer = Stopwatch.StartNew();

		CreateDungeon(20);

		currentRoomIndex = 0;
		roomList[0].PlayerEntered();

		player.Show();
		player.MoveToFront();

		gameStarted = true;
		SetProcess(true);

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
		gameTimer.Stop();
		gameStarted = false;
		GD.Print(gameTimer.Elapsed.TotalSeconds);
		if (succeeded)
		{
			EmitSignal(SignalName.GoToRunMenu);
		}
		else
		{
			EmitSignal(SignalName.GoToMainMenu);
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
