using Godot;
using System;
using System.Collections.Generic;
using System.Data;

public partial class Items : Node2D
{
	Random rn = new Random();

	public enum Sprites
	{
		NONE = 0,
		BIG_COBWEB = 1,
		SMALL_COBWEB = 2,
		BONES = 3,
		SKULL_AND_BONES = 4,
		SMALL_BOX = 5,
		BIG_BOX = 6,
		SMALL_CHEST = 7,
		BIG_CHEST = 8,
		KEY = 9
	}
	
	public Sprites[,] spriteMap;
	
	private Vector2I SpriteCoords(Sprites spr)
	{
		switch (spr)
		{
			case Sprites.BIG_COBWEB:
				return new Vector2I(4, 6);
			case Sprites.SMALL_COBWEB:
				return new Vector2I(5, 6);
			case Sprites.BONES:
				return new Vector2I(8, 6);
			case Sprites.SKULL_AND_BONES:
				return new Vector2I(7, 7);
			case Sprites.SMALL_BOX:
				return new Vector2I(0, 8);
			case Sprites.BIG_BOX:
				return new Vector2I(3, 8);
			case Sprites.SMALL_CHEST:
				return new Vector2I(5, 8);
			case Sprites.BIG_CHEST:
				return new Vector2I(4, 8);
			default: //key
				return new Vector2I(8, 8);
		}
	}
	
	public void ShowItems()
	{
		itemLayout.Show();
		itemLayout.CollisionEnabled = true;
	}

	public void HideItems()
	{
		itemLayout.Hide();
		itemLayout.CollisionEnabled = false;
	}

	private bool ValidPlacement(int xSize, int ySize, int x, int y, List<Room.Dir> dirs)
	{
		foreach(Room.Dir d in dirs)
		{
			switch (d)
			{
				case Room.Dir.UP:
					if ((x == ((xSize - 2) / 2) || x == ((xSize - 2) / 2) - 1) && y == 0) return false;
					break;
				case Room.Dir.DOWN:
					if ((x == ((xSize - 2) / 2) || x == ((xSize - 2) / 2) - 1) && y == ySize - 3) return false;
					break;
				case Room.Dir.LEFT:
					if (x == 0 && (y == ((ySize - 2) / 2) || y == ((ySize - 2) / 2 - 1))) return false;
					break;
				case Room.Dir.RIGHT:
					if (x == xSize - 3 && (y == ((ySize - 2) / 2) || y == ((ySize - 2) / 2 - 1))) return false;
					break;
			}
		}
		return true;
	}

	public void MakeItems(int xStart, int xSize, int yStart, int ySize, List<Room.Dir> connectedRooms)
	{
		spriteMap = new Sprites[xSize - 2, ySize - 2];
		int roomSize = (xSize - 2) * (ySize - 2);
		int numberOfItems = roomSize / 16;
		int x, y;
		Sprites newSpr;
		bool isValidDirection, isValidPlace;
		for (int i = 0; i < numberOfItems; i++)
		{
			if (rn.Next(1,101) == 19) newSpr = Items.Sprites.KEY;
			else newSpr = (Sprites)rn.Next(1,9);

			if (newSpr == Sprites.BIG_COBWEB || newSpr == Sprites.SMALL_COBWEB) isValidDirection = false;
			else isValidDirection = true;

			isValidPlace = false;

			do
			{
				x = rn.Next(0, xSize - 2);
				y = rn.Next(0, ySize - 2);
				if (!isValidDirection && (x == 0 || x == xSize - 3)) isValidDirection = true;
				if (ValidPlacement(xSize, ySize, x, y, connectedRooms)) isValidPlace = true;
			} while (spriteMap[x, y] != Sprites.NONE || !isValidDirection | !isValidPlace);

			spriteMap[x,y] = newSpr;
			if ((newSpr == Sprites.BIG_COBWEB && x == xSize - 3) || (newSpr == Sprites.SMALL_COBWEB && x == 0))
			{
				itemLayout.SetCell(new Vector2I(x + xStart + 2, y + yStart + 2), 0, SpriteCoords(newSpr), (int)TileSetAtlasSource.TransformFlipH);
			}
			else
			{
				itemLayout.SetCell(new Vector2I(x + xStart + 2, y + yStart + 2), 0, SpriteCoords(newSpr));
			}
		}
	}

	public void StartDoors(int x, int y)
	{
		itemLayout.SetCell(new Vector2I(x, y), 0, new Vector2I(6, 6));
		itemLayout.SetCell(new Vector2I(x + 1, y), 0, new Vector2I(7, 6));
	}

	public TileMapLayer itemLayout;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		itemLayout = GetNode<TileMapLayer>("ItemsLayout");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
