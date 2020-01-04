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
        this.LoadFloor(file);
        try
        {

        }
        catch
        {
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
        string[] strArray2;
        this.keys[0] = 0;
        this.keys[1] = 0;
        if (path == "")
        {
            path = file;
        }
        char[] separator = new char[] { '|' };
        string[] strArray = Crypt.Read(path).Split(separator);
        int index = 0;
        char[] chArray2 = new char[] { ' ' };
        int num2 = int.Parse(strArray[index].Split(chArray2)[0]);
        index++;
        this.map = new int[num2, 8];
        this.floor = new int[num2][][,];
        for (int i = 0; i < num2; i++)
        {
            this.floor[i] = new int[2][,];
            char[] chArray3 = new char[] { ' ' };
            strArray2 = strArray[index].Split(chArray3);
            index++;
            for (int k = 0; k < 8; k++)
            {
                this.map[i, k] = int.Parse(strArray2[k]);
            }
        }
        char[] chArray4 = new char[] { ' ' };
        strArray2 = strArray[index].Split(chArray4);
        index++;
        roomid[0] = int.Parse(strArray2[0]);
        roomid[1] = int.Parse(strArray2[1]);
        for (int j = 0; j < num2; j++)
        {
            char[] chArray5 = new char[] { ' ' };
            strArray2 = strArray[index].Split(chArray5);
            index++;
            int num6 = int.Parse(strArray2[0]);
            int num7 = int.Parse(strArray2[1]);
            for (int k = 0; k < 2; k++)
            {
                this.floor[j][k] = new int[num6, num7];
                for (int m = 0; m < num7; m++)
                {
                    char[] chArray6 = new char[] { ' ' };
                    strArray2 = strArray[index].Split(chArray6);
                    index++;
                    for (int n = 0; n < num6; n++)
                    {
                        if ((this.floor[j][k][n, m] = int.Parse(strArray2[n])) == 12)
                        {
                            this.keys[0]++;
                        }
                    }
                }
            }
        }
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
            RObjects[level][x, y].GetComponent<CellPos>().Move(dx, dy);
            RObjects[level][x + dx, y + dy] = RObjects[level][x, y];
            RObjects[level][x, y] = null;
            thisRoom[level][x + dx, y + dy] = thisRoom[level][x, y];
            thisRoom[level][x, y] = 0;
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
            int[] p = new int[2];
            switch (id)
            {
                case 0:
                    for (int i = 0; i < this.maxX; i++)
                    {
                        if (door[R[0][i, this.maxY - 1]])
                        {
                            p[0] = i;
                        }
                    }
                    p[1] = this.maxY - 1;
                    break;

                case 1:
                    for (int i = 0; i < this.maxX; i++)
                    {
                        if (door[R[0][i, 0]])
                        {
                            p[0] = i;
                        }
                    }
                    p[1] = 0;
                    break;

                case 2:
                    for (int i = 0; i < this.maxY; i++)
                    {
                        if (door[R[0][this.maxX - 1, i]])
                        {
                            p[1] = i;
                        }
                    }
                    p[0] = this.maxX - 1;
                    break;

                case 3:
                    for (int i = 0; i < this.maxY; i++)
                    {
                        if (door[R[0][0, i]])
                        {
                            p[1] = i;
                        }
                    }
                    p[0] = 0;
                    break;
            }
            if (((p[0] >= 0) && (p[1] >= 0)) && intangible[R[1][p[0], p[1]]])
            {
                return p;
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
        string str2;
        string data = "";
        int length = this.floor.Length;
        data = data + ((int)length).ToString() + " -|";
        for (int i = 0; i < length; i++)
        {
            str2 = "";
            for (int k = 0; k < 8; k++)
            {
                str2 = str2 + ((int)this.map[i, k]).ToString();
                if (k < 7)
                {
                    str2 = str2 + " ";
                }
            }
            data = data + str2 + "|";
        }
        string[] textArray1 = new string[] { data, ((int)roomid[0]).ToString(), " ", ((int)roomid[1]).ToString(), "|" };
        data = string.Concat((string[])textArray1);
        for (int j = 0; j < length; j++)
        {
            int num5 = this.floor[j][0].GetLength(0);
            int num6 = this.floor[j][0].GetLength(1);
            string[] textArray2 = new string[] { data, ((int)num5).ToString(), " ", ((int)num6).ToString(), "|" };
            data = string.Concat((string[])textArray2);
            for (int k = 0; k < 2; k++)
            {
                for (int m = 0; m < num6; m++)
                {
                    str2 = "";
                    for (int n = 0; n < num5; n++)
                    {
                        str2 = str2 + ((int)this.floor[j][k][n, m]);
                        if (n < (num5 - 1))
                        {
                            str2 = str2 + " ";
                        }
                    }
                    data = data + str2 + "|";
                }
            }
        }
        Crypt.Write(path, data);
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
        string str = Crypt.Read(file);
        Crypt.Write(file, str.Replace("-", "+"));
    }
}