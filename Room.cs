using Godot;
using System;
using System.Collections.Generic;
using System.Net;
using System.Numerics;

public partial class Room : Node2D
{
	[Export]
    public PackedScene ItemScene { get; set; }

	public Items roomItems;

	public bool isStart;
	public List<Dir> connectedRooms = [];

	public enum Dir 
	{
		UP = 0, 
		DOWN = 1, 
		LEFT = 2, 
		RIGHT = 3
	};
	enum Tiles 
	{
		OUTSIDE_BOUNDARY = 0, 
		FLOOR = 1, 
		L_WALL = 2, 
		R_WALL = 3,
		T_WALL = 4,
		B_WALL = 5,
		BL_CORNER = 6,
		BR_CORNER = 7,
		LEFT_CONNECTOR_B = 8,
		LEFT_CONNECTOR_T = 9,
		RIGHT_CONNECTOR_B = 10,
		RIGHT_CONNECTOR_T = 11,
		TOP_CONNECTOR_L = 12,
		TOP_CONNECTOR_R = 13,
		BOTTOM_CONNECTOR_L = 14,
		BOTTOM_CONNECTOR_R = 15,
		ENTRANCE_L = 16,
		ENTRANCE_R = 17
	}

	Random rn = new Random();

	const int MAX_X_SIZE = 16, MAX_Y_SIZE = 12;	
	public int xSize, xStart, ySize, yStart;

	public TileMapLayer roomLayout;

	public void PlayerEntered()
	{
		roomLayout.Show();
		roomLayout.CollisionEnabled = true;
		roomItems.ShowItems();
	}

	public void PlayerLeft()
	{
		roomLayout.Hide();
		roomLayout.CollisionEnabled = false;
		roomItems.HideItems();
	}

	private Vector2I randomTileCoords(Tiles type)
	{
		switch (type)
		{
			case Tiles.FLOOR:
				return new Vector2I(rn.Next(6, 10), rn.Next(0, 3));
			case Tiles.L_WALL:
				return new Vector2I(0, rn.Next(0, 4));
			case Tiles.R_WALL:
				return new Vector2I(5, rn.Next(0, 4));
			case Tiles.T_WALL:
				return new Vector2I(rn.Next(1, 5), 0);
			case Tiles.B_WALL:
				return new Vector2I(rn.Next(1, 5), 4);
			case Tiles.BL_CORNER:
				return new Vector2I(0, 4);
			case Tiles.BR_CORNER:
				return new Vector2I(5, 4);
			case Tiles.LEFT_CONNECTOR_B:
				return new Vector2I(5, 5);
			case Tiles.LEFT_CONNECTOR_T:
				return new Vector2I(rn.Next(1, 5), 0);
			case Tiles.RIGHT_CONNECTOR_B:
				return new Vector2I(4, 5);
			case Tiles.RIGHT_CONNECTOR_T:
				return new Vector2I(rn.Next(1, 5), 0);
			case Tiles.TOP_CONNECTOR_L:
				return new Vector2I(rn.Next(1, 5), 0);
			case Tiles.TOP_CONNECTOR_R:
				return new Vector2I(rn.Next(1, 5), 0);
			case Tiles.BOTTOM_CONNECTOR_L:
				return new Vector2I(5, 5);
			case Tiles.BOTTOM_CONNECTOR_R:
				return new Vector2I(4, 5);
			case Tiles.ENTRANCE_L:
				return new Vector2I(rn.Next(6, 10), rn.Next(0, 3));
			case Tiles.ENTRANCE_R:
				return new Vector2I(rn.Next(6, 10), rn.Next(0, 3));
			default: //outside
				return new Vector2I(8, 7);
		}
	}

	private void DisplayRoom(TileMapLayer layout, Tiles[,] roomArray)
	{
		for (int x = 0; x < MAX_X_SIZE + 2; x++)
		{
			for (int y = 0; y < MAX_Y_SIZE + 2; y++)
			{
				if (x == 0 || y == 0 || x == MAX_X_SIZE + 1 || y == MAX_Y_SIZE + 1) //boundary
				{
					layout.SetCell(new Vector2I(x, y), 0, new Vector2I(8, 7));
				}
				else
				{
					Vector2I tilePos = randomTileCoords(roomArray[x - 1, y - 1]);
					layout.SetCell(new Vector2I(x, y), 0, tilePos);
				}
			}
		}
	}

	private Tiles[,] AddConnections(Tiles[,] roomArray, List<Dir> dirs)
	{
		foreach(Dir d in dirs)
		{
			switch (d)
			{
				case Dir.UP:
					roomArray[xStart + (xSize / 2) - 2, yStart] = Tiles.TOP_CONNECTOR_L;
					roomArray[xStart + (xSize / 2) - 1, yStart] = Tiles.FLOOR;
					roomArray[xStart + (xSize / 2), yStart] = Tiles.FLOOR;
					roomArray[xStart + (xSize / 2) + 1, yStart] = Tiles.TOP_CONNECTOR_R;
					break;
				case Dir.DOWN:
					roomArray[xStart + (xSize / 2) - 2, yStart + ySize - 1] = Tiles.BOTTOM_CONNECTOR_L;
					roomArray[xStart + (xSize / 2) - 1, yStart + ySize - 1] = Tiles.FLOOR;
					roomArray[xStart + (xSize / 2), yStart + ySize - 1] = Tiles.FLOOR;
					roomArray[xStart + (xSize / 2) + 1, yStart + ySize - 1] = Tiles.BOTTOM_CONNECTOR_R;
					break;
				case Dir.LEFT:
					roomArray[xStart, yStart + (ySize / 2) - 2] = Tiles.LEFT_CONNECTOR_T;
					roomArray[xStart, yStart + (ySize / 2) - 1] = Tiles.FLOOR;
					roomArray[xStart, yStart + (ySize / 2)] = Tiles.FLOOR;
					roomArray[xStart, yStart + (ySize / 2) + 1] = Tiles.LEFT_CONNECTOR_B;
					break;
				case Dir.RIGHT:
					roomArray[xStart + xSize - 1, yStart + (ySize / 2) - 2] = Tiles.RIGHT_CONNECTOR_T;
					roomArray[xStart + xSize - 1, yStart + (ySize / 2) - 1] = Tiles.FLOOR;
					roomArray[xStart + xSize - 1, yStart + (ySize / 2)] = Tiles.FLOOR;
					roomArray[xStart + xSize - 1, yStart + (ySize / 2) + 1] = Tiles.RIGHT_CONNECTOR_B;
					break;
			}
		}
		return roomArray;
	}

	private Tiles[,] GenerateRoom(List<Dir> dirs)
	{
		Tiles[,] roomArray = new Tiles[MAX_X_SIZE, MAX_Y_SIZE];

		// need room size to be even so it is exactly in the centre
		xSize = rn.Next(3, (MAX_X_SIZE / 2) + 1) * 2;
		ySize = rn.Next(3, (MAX_Y_SIZE / 2) + 1) * 2;

		xStart = 8 - (xSize / 2);
		yStart = 6 - (ySize / 2);

		for (int x = 0; x < MAX_X_SIZE; x++)
		{
			for (int y = 0; y < MAX_Y_SIZE; y++)
			{
				if (x > xStart && x < xStart + xSize - 1 && y > yStart && y < yStart + ySize - 1) //floor
				{
					roomArray[x,y] = Tiles.FLOOR;
				}
				else if (x == xStart && y == yStart + ySize - 1) //bottom left corner
				{
					roomArray[x,y] = Tiles.BL_CORNER;
				}
				else if (x == xStart + xSize - 1 && y == yStart + ySize - 1) //bottom right corner
				{
					roomArray[x,y] = Tiles.BR_CORNER;
				}
				else if (x == xStart && y >= yStart && y < yStart + ySize - 1) //left wall
				{
					roomArray[x,y] = Tiles.L_WALL;
				}
				else if (x == xStart + xSize - 1 && y >= yStart && y < yStart + ySize - 1) //right wall
				{
					roomArray[x,y] = Tiles.R_WALL;
				}
				else if (x > xStart && x < xStart + xSize - 1 && y == yStart) //top wall
				{
					roomArray[x,y] = Tiles.T_WALL;
				}
				else if (x > xStart && x < xStart + xSize - 1 && y == yStart + ySize - 1) //bottom wall
				{
					roomArray[x,y] = Tiles.B_WALL;
				}
				else //boundary
				{
					roomArray[x,y] = Tiles.OUTSIDE_BOUNDARY;
				}
			}
		}

		roomArray = AddConnections(roomArray, dirs);

		return roomArray;
	}
	
	public override void _Ready()
	{
		roomLayout = GetNode<TileMapLayer>("RoomLayout");

		Tiles[,] roomArray = GenerateRoom(connectedRooms);
		DisplayRoom(roomLayout, roomArray);

		roomItems = ItemScene.Instantiate<Items>();
		AddChild(roomItems);
		roomItems.MakeItems(xStart, xSize, yStart, ySize, connectedRooms);
		if (isStart)
		{
			roomItems.StartDoors(xStart + (xSize / 2), yStart + ySize);
		}

		PlayerLeft();
	}

	public override void _Process(double delta)
	{
		
	}
}
