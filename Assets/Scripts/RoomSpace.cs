using System;
using System.Collections;
using UnityEngine;

public class RoomSpace : MonoBehaviour
{
	public const float cellsizeX = 1.15f;

	public const float cellsizeY = 0.78f;

	public GameObject[] objects;

	private GameObject[][,] RObjects = new GameObject[2][,];

	public int[][][,] floor;

	public int[][,] thisRoom = new int[2][,];

	public int tasks;

	public int[] keys = new int[2];

	public int[,] map;

	public int maxX;

	public int maxY;

	public static int[] roomid = new int[2];

	public static RoomSpace M;

	public static string file;

	public static BitArray intangible;

	public static BitArray bigobject;

	public static BitArray movable;

	public static BitArray killing;

	public static BitArray door;

	private void Awake()
	{
		RoomSpace.M = this;
		this.RObjects[0] = new GameObject[0, 0];
		this.RObjects[1] = new GameObject[0, 0];
		RoomSpace.intangible = new BitArray(this.objects.Length + 2);
		RoomSpace.bigobject = new BitArray(this.objects.Length + 2);
		RoomSpace.movable = new BitArray(this.objects.Length + 2);
		RoomSpace.killing = new BitArray(this.objects.Length + 2);
		RoomSpace.door = new BitArray(this.objects.Length + 2);
		RoomSpace.intangible[0] = true;
		RoomSpace.intangible[12] = true;
		RoomSpace.movable[4] = true;
		RoomSpace.movable[12] = true;
		RoomSpace.bigobject[3] = true;
		RoomSpace.bigobject[5] = true;
		RoomSpace.killing[7] = true;
		RoomSpace.killing[8] = true;
		RoomSpace.door[5] = true;
		RoomSpace.door[11] = true;
		try
		{
			this.LoadFloor(RoomSpace.file);
		}
		catch
		{
			Error.ErrorMassage("Ошибка чтения файла. Возможно он был поврежден");
		}
	}

	public void PrintRoom(int room, int pos)
	{
        thisRoom = new int[2][,];
		int [][,] R = this.floor[room];
		this.maxX = R[0].GetLength(0);
		this.maxY = R[0].GetLength(1);
		int[] array = FindDoor(pos);
		if (array == null)
		{
			return;
		}
		int num = array[0];
		int num2 = array[1];
        Clear();
        this.thisRoom[0] = new int[this.maxX, this.maxY];
		this.thisRoom[1] = new int[this.maxX, this.maxY];
		this.RObjects[0] = new GameObject[this.maxX, this.maxY];
		this.RObjects[1] = new GameObject[this.maxX, this.maxY];
		for (int i = 0; i < this.maxX; i++)
		{
			for (int j = 0; j < this.maxY; j++)
			{
				for (int k = 0; k < 2; k++)
				{
					this.thisRoom[k][i, j] = R[k][i, j];
					if (R[k][i, j] > 0 && R[k][i, j] <= this.objects.Length && R[k][i, j] != 2)
					{
						GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.objects[R[k][i, j] - 1]);
						gameObject.GetComponent<CellPos>().SetPos(i, j, (float)k * 0.5f);
						this.RObjects[k][i, j] = gameObject;
					}
				}
			}
		}
		this.objects[1].GetComponent<CellPos>().SetPos(num, num2, 0.5f);
		this.objects[1].GetComponent<Rotation>().rotation = Rotation.Flip(pos);
		this.RObjects[1][num, num2] = this.objects[1];
		this.SetCell(num, num2, 2, 1);
        void Clear()
        {

        }
        int[] FindDoor(int id)
        {
            int[] a = new int[2];
            return a;
        }
	}

	public int Move(int x, int y, int dx, int dy, int level = 1)
	{
		if (x + dx < 0 || y + dy < 0 || x + dx >= this.thisRoom[0].GetLength(0) || y + dy >= this.thisRoom[0].GetLength(1))
		{
			return 2;
		}
		if (this.GetCell(x + dx, y + dy, level) >= 0 && RoomSpace.intangible[this.thisRoom[level][x + dx, y + dy]] && this.GetCell(x + dx, y + dy, 0) > 0)
		{
			this.RObjects[level][x, y].GetComponent<CellPos>().Move(dx, dy);
			this.RObjects[level][x + dx, y + dy] = this.RObjects[level][x, y];
			this.RObjects[level][x, y] = null;
			this.thisRoom[level][x + dx, y + dy] = this.thisRoom[level][x, y];
			this.thisRoom[level][x, y] = 0;
			return 0;
		}
		return 1;
	}

	public int GetCell(int x, int y, int level = 1)
	{
		if (x >= 0 && y >= 0 && x < this.thisRoom[0].GetLength(0) && y < this.thisRoom[0].GetLength(1))
		{
			return this.thisRoom[level][x, y];
		}
		return -1;
	}

	public GameObject GetObject(int x, int y, int level = 1)
	{
		if (x >= 0 && y >= 0 && x < this.thisRoom[level].GetLength(0) && y < this.thisRoom[level].GetLength(1) && this.GetCell(x, y, level) != 0)
		{
			return this.RObjects[level][x, y];
		}
		return null;
	}

	public void SetCell(int x, int y, int v, int level = 1)
	{
		this.thisRoom[level][x, y] = v;
	}

	public GameObject SetObject(int x, int y, int v, int level = 1)
	{
		UnityEngine.Object.Destroy(this.RObjects[level][x, y]);
		this.thisRoom[level][x, y] = v;
		if (v > 0 && v <= this.objects.Length)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.objects[v - 1]);
			gameObject.GetComponent<CellPos>().SetPos(x, y, (float)level * 0.5f);
			this.RObjects[level][x, y] = gameObject;
			return gameObject;
		}
		return null;
	}

	public void LoadFloor(string path = "lastsave.ncs")
	{
		this.keys[0] = 0;
		this.keys[1] = 0;
		if (path == "")
		{
			path = RoomSpace.file;
		}
		string[] array = Crypt.Read(path).Split(new char[]
		{
			'|'
		});
		int num = 0;
		int num2 = int.Parse(array[num].Split(new char[]
		{
			' '
		})[0]);
		num++;
		this.map = new int[num2, 8];
		this.floor = new int[num2][][,];
		string[] array2;
		for (int i = 0; i < num2; i++)
		{
			this.floor[i] = new int[2][,];
			array2 = array[num].Split(new char[]
			{
				' '
			});
			num++;
			for (int j = 0; j < 8; j++)
			{
				this.map[i, j] = int.Parse(array2[j]);
			}
		}
		array2 = array[num].Split(new char[]
		{
			' '
		});
		num++;
		RoomSpace.roomid[0] = int.Parse(array2[0]);
		RoomSpace.roomid[1] = int.Parse(array2[1]);
		for (int k = 0; k < num2; k++)
		{
			array2 = array[num].Split(new char[]
			{
				' '
			});
			num++;
			int num3 = int.Parse(array2[0]);
			int num4 = int.Parse(array2[1]);
			for (int l = 0; l < 2; l++)
			{
				this.floor[k][l] = new int[num3, num4];
				for (int m = 0; m < num4; m++)
				{
					array2 = array[num].Split(new char[]
					{
						' '
					});
					num++;
					for (int n = 0; n < num3; n++)
					{
						if ((this.floor[k][l][n, m] = int.Parse(array2[n])) == 12)
						{
							this.keys[0]++;
						}
					}
				}
			}
		}
		this.PrintRoom(RoomSpace.roomid[0], RoomSpace.roomid[1]);
	}

	public void SaveRoom()
	{
		int num = this.maxX;
		int num2 = this.maxY;
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				for (int k = 0; k < num; k++)
				{
					if (this.thisRoom[i][k, j] != 2)
					{
						this.floor[RoomSpace.roomid[0]][i][k, j] = this.thisRoom[i][k, j];
					}
				}
			}
		}
	}

	public void NextRoom(int door)
	{
		this.keys[0] -= this.keys[1];
		this.keys[1] = 0;
		int num = this.map[RoomSpace.roomid[0], door] - 1;
		if (this.map[RoomSpace.roomid[0], door] != 0)
		{
			this.SaveRoom();
			RoomSpace.roomid[1] = this.map[RoomSpace.roomid[0], door + 4];
			RoomSpace.roomid[0] = num;
			this.PrintRoom(RoomSpace.roomid[0], RoomSpace.roomid[1]);
		}
	}

	public void SaveGame(string path = "lastsave.ncs")
	{
		string text = "";
		int num = this.floor.Length;
		text = text + num.ToString() + " -|";
		for (int i = 0; i < num; i++)
		{
			string text2 = "";
			for (int j = 0; j < 8; j++)
			{
				text2 += this.map[i, j].ToString();
				if (j < 7)
				{
					text2 += " ";
				}
			}
			text = text + text2 + "|";
		}
		text = string.Concat(new string[]
		{
			text,
			RoomSpace.roomid[0].ToString(),
			" ",
			RoomSpace.roomid[1].ToString(),
			"|"
		});
		for (int k = 0; k < num; k++)
		{
			int length = this.floor[k][0].GetLength(0);
			int length2 = this.floor[k][0].GetLength(1);
			text = string.Concat(new string[]
			{
				text,
				length.ToString(),
				" ",
				length2.ToString(),
				"|"
			});
			for (int l = 0; l < 2; l++)
			{
				for (int m = 0; m < length2; m++)
				{
					string text2 = "";
					for (int n = 0; n < length; n++)
					{
						text2 += this.floor[k][l][n, m];
						if (n < length - 1)
						{
							text2 += " ";
						}
					}
					text = text + text2 + "|";
				}
			}
		}
		Crypt.Write(path, text);
	}

	public void Victory()
	{
		string text = Crypt.Read(RoomSpace.file);
		Crypt.Write(RoomSpace.file, text.Replace("-", "+"));
	}
}
