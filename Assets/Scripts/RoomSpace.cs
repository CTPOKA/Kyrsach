using System.Collections;
using System.IO;
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
        M = this;
        this.RObjects[0] = new GameObject[0, 0];
        this.RObjects[1] = new GameObject[0, 0];
        intangible = new BitArray(this.objects.Length + 2);
        bigobject = new BitArray(this.objects.Length + 2);
        movable = new BitArray(this.objects.Length + 2);
        killing = new BitArray(this.objects.Length + 2);
        door = new BitArray(this.objects.Length + 2);
        intangible[0] = true;
        intangible[12] = true;
        movable[4] = true;
        movable[12] = true;
        bigobject[3] = true;
        bigobject[5] = true;
        killing[7] = true;
        killing[8] = true;
        door[5] = true;
        door[11] = true;
        try
        {
            this.LoadFloor(file);
        }
        catch
        {
            Error.ErrorMassage("Ошибка чтения файла. Возможно он был поврежден");
        }
    }

    public int GetCell(int x, int y, int level = 1)
    {
        if (((x >= 0) && (y >= 0)) && ((x < this.thisRoom[0].GetLength(0)) && (y < this.thisRoom[0].GetLength(1))))
        {
            return this.thisRoom[level][x, y];
        }
        return -1;
    }

    public GameObject GetObject(int x, int y, int level = 1)
    {
        if ((((x >= 0) && (y >= 0)) && ((x < this.thisRoom[level].GetLength(0)) && (y < this.thisRoom[level].GetLength(1)))) && (this.GetCell(x, y, level) != 0))
        {
            return this.RObjects[level][x, y];
        }
        return null;
    }

    public void LoadFloor(string path = "lastsave.ncs")
    {
        string[] strArray;
        this.keys[0] = 0;
        this.keys[1] = 0;
        if (path == "")
        {
            path = file;
        }
        StreamReader reader = new StreamReader(Application.dataPath + "/Saves/" + path);
        char[] separator = new char[] { ' ' };
        int num = int.Parse(reader.ReadLine().Split(separator)[0]);
        this.map = new int[num, 8];
        floor = new int[num][][,];
        for (int i = 0; i < num; i++)
        {
            this.floor[i] = new int[2][,];
            char[] chArray2 = new char[] { ' ' };
            strArray = reader.ReadLine().Split(chArray2);
            for (int k = 0; k < 8; k++)
            {
                this.map[i, k] = int.Parse(strArray[k]);
            }
        }
        char[] chArray3 = new char[] { ' ' };
        strArray = reader.ReadLine().Split(chArray3);
        roomid[0] = int.Parse(strArray[0]);
        roomid[1] = int.Parse(strArray[1]);
        for (int j = 0; j < num; j++)
        {
            char[] chArray4 = new char[] { ' ' };
            strArray = reader.ReadLine().Split(chArray4);
            int num5 = int.Parse(strArray[0]);
            int num6 = int.Parse(strArray[1]);
            for (int k = 0; k < 2; k++)
            {
                this.floor[j][k] = new int[num5, num6];
                for (int m = 0; m < num6; m++)
                {
                    char[] chArray5 = new char[] { ' ' };
                    strArray = reader.ReadLine().Split(chArray5);
                    for (int n = 0; n < num5; n++)
                    {
                        if ((this.floor[j][k][n, m] = int.Parse(strArray[n])) == 12)
                        {
                            this.keys[0]++;
                        }
                    }
                }
            }
        }
        reader.Close();
        this.PrintRoom(roomid[0], roomid[1]);
    }

    public int Move(int x, int y, int dx, int dy, int level = 1)
    {
        if ((((x + dx) < 0) || ((y + dy) < 0)) || (((x + dx) >= this.thisRoom[0].GetLength(0)) || ((y + dy) >= this.thisRoom[0].GetLength(1))))
        {
            return 2;
        }
        if (((this.GetCell(x + dx, y + dy, level) >= 0) && intangible[this.thisRoom[level][x + dx, y + dy]]) && (this.GetCell(x + dx, y + dy, 0) > 0))
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

    public void NextRoom(int door)
    {
        this.keys[0] -= this.keys[1];
        this.keys[1] = 0;
        int num = this.map[roomid[0], door] - 1;
        if (this.map[roomid[0], door] != 0)
        {
            this.SaveRoom();
            roomid[1] = this.map[roomid[0], door + 4];
            roomid[0] = num;
            this.PrintRoom(roomid[0], roomid[1]);
        }
    }

    public void PrintRoom(int room, int pos)
    {
        int[][,] R;
        int[] FindDoor(int id)
        {
            int[] numArray2 = new int[] { -1, -1 };
            switch (id)
            {
                case 0:
                    for (int i = 0; i < this.maxX; i++)
                    {
                        if (door[R[0][i, this.maxY - 1]])
                        {
                            numArray2[0] = i;
                        }
                    }
                    numArray2[1] = this.maxY - 1;
                    break;

                case 1:
                    for (int i = 0; i < this.maxX; i++)
                    {
                        if (door[R[0][i, 0]])
                        {
                            numArray2[0] = i;
                        }
                    }
                    numArray2[1] = 0;
                    break;

                case 2:
                    for (int i = 0; i < this.maxY; i++)
                    {
                        if (door[R[0][this.maxX - 1, i]])
                        {
                            numArray2[1] = i;
                        }
                    }
                    numArray2[0] = this.maxX - 1;
                    break;

                case 3:
                    for (int i = 0; i < this.maxY; i++)
                    {
                        if (door[R[0][0, i]])
                        {
                            numArray2[1] = i;
                        }
                    }
                    numArray2[0] = 0;
                    break;
            }
            if (((numArray2[0] >= 0) && (numArray2[1] >= 0)) && intangible[R[1][numArray2[0], numArray2[1]]])
            {
                return numArray2;
            }
            return null;
        }
        void ClearRoom()
        {
            this.tasks = 0;
            for (int i = 0; i < this.RObjects[0].GetLength(0); i++)
            {
                for (int j = 0; j < this.RObjects[0].GetLength(1); j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        if (this.RObjects[k][i, j] != this.objects[1])
                        {
                            Destroy(this.RObjects[k][i, j]);
                        }
                    }
                }
            }
        }
        R = this.floor[room];
        this.maxX = R[0].GetLength(0);
        this.maxY = R[0].GetLength(1);
        int[] numArray = FindDoor(pos);
        if (numArray != null)
        {
            int x = numArray[0];
            int y = numArray[1];
            ClearRoom();
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
                        if (((R[k][i, j] > 0) && (R[k][i, j] <= this.objects.Length)) && (R[k][i, j] != 2))
                        {
                            GameObject obj2 = Instantiate<GameObject>(this.objects[R[k][i, j] - 1]);
                            obj2.GetComponent<CellPos>().SetPos(i, j, k * 0.5f);
                            this.RObjects[k][i, j] = obj2;
                        }
                    }
                }
            }
            this.objects[1].GetComponent<CellPos>().SetPos(x, y, 0.5f);
            this.objects[1].GetComponent<Rotation>().rotation = Rotation.Flip(pos);
            this.RObjects[1][x, y] = this.objects[1];
            this.SetCell(x, y, 2, 1);
        }
    }

    public void SaveGame(string path = "lastsave.ncs")
    {
        string str;
        StreamWriter writer = new StreamWriter(Application.dataPath + "/Saves/" + path);
        int length = this.floor.Length;
        writer.WriteLine(((int)length).ToString() + " -");
        for (int i = 0; i < length; i++)
        {
            str = "";
            for (int k = 0; k < 8; k++)
            {
                str = str + ((int)this.map[i, k]).ToString();
                if (k < 7)
                {
                    str = str + " ";
                }
            }
            writer.WriteLine(str);
        }
        writer.WriteLine(((int)roomid[0]).ToString() + " " + ((int)roomid[1]).ToString());
        for (int j = 0; j < length; j++)
        {
            int num5 = this.floor[j][0].GetLength(0);
            int num6 = this.floor[j][0].GetLength(1);
            writer.WriteLine(((int)num5).ToString() + " " + ((int)num6).ToString());
            for (int k = 0; k < 2; k++)
            {
                for (int m = 0; m < num6; m++)
                {
                    str = "";
                    for (int n = 0; n < num5; n++)
                    {
                        str = str + ((int)this.floor[j][k][n, m]);
                        if (n < (num5 - 1))
                        {
                            str = str + " ";
                        }
                    }
                    writer.WriteLine(str);
                }
            }
        }
        writer.Close();
    }

    public void SaveRoom()
    {
        int maxX = this.maxX;
        int maxY = this.maxY;
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < maxY; j++)
            {
                for (int k = 0; k < maxX; k++)
                {
                    if (this.thisRoom[i][k, j] != 2)
                    {
                        this.floor[roomid[0]][i][k, j] = this.thisRoom[i][k, j];
                    }
                }
            }
        }
    }

    public void SetCell(int x, int y, int v, int level = 1)
    {
        this.thisRoom[level][x, y] = v;
    }

    public GameObject SetObject(int x, int y, int v, int level = 1)
    {
        Destroy(this.RObjects[level][x, y]);
        this.thisRoom[level][x, y] = v;
        if ((v > 0) && (v <= this.objects.Length))
        {
            GameObject obj2 = Instantiate<GameObject>(this.objects[v - 1]);
            obj2.GetComponent<CellPos>().SetPos(x, y, level * 0.5f);
            this.RObjects[level][x, y] = obj2;
            return obj2;
        }
        return null;
    }

    public void Victory()
    {
        StreamReader reader1 = new StreamReader(Application.dataPath + "/Saves/" + file);
        string str = reader1.ReadToEnd();
        reader1.Close();
        StreamWriter writer1 = new StreamWriter(Application.dataPath + "/Saves/" + file);
        writer1.Write(str.Replace("-", "+"));
        writer1.Close();
    }
}
