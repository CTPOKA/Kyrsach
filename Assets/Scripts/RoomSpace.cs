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
    public static string file;
    public static bool newgame;
    public static RoomSpace M;
    public static BitArray intangible;
    public static BitArray bigobject;
    public static BitArray movable;
    public static BitArray killing;
    public static BitArray door;

    private void Awake()
    {
        M = this;
        RObjects[0] = new GameObject[0, 0];
        RObjects[1] = new GameObject[0, 0];
        intangible = new BitArray(objects.Length + 2);
        bigobject = new BitArray(objects.Length + 2);
        movable = new BitArray(objects.Length + 2);
        killing = new BitArray(objects.Length + 2);
        door = new BitArray(objects.Length + 2);
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
        if (newgame)
        {
            LoadFloor(file);
            newgame = false;
        }
        else LoadFloor();
    }

    public int GetCell(int x, int y, int level = 1)
    {
        if ((x >= 0) && (y >= 0) && (x < thisRoom[0].GetLength(0)) && (y < thisRoom[0].GetLength(1)))
        {
            return thisRoom[level][x, y];
        }
        return -1;
    }

    public GameObject GetObject(int x, int y, int level = 1)
    {
        if ((x >= 0) && (y >= 0) && (x < thisRoom[level].GetLength(0)) && (y < thisRoom[level].GetLength(1)) && (this.GetCell(x, y, level) != 0))
        {
            return RObjects[level][x, y];
        }
        return null;
    }

    public void LoadFloor(string path = "lastsave.ncs")
    {
        string[] tmpstr;
        this.keys[0] = 0;
        this.keys[1] = 0;
        if (path == "")
        {
            path = file;
        }
        string[] strArray = Crypt.Read(path).Split('|');
        int index = 0;
        int num2 = int.Parse(strArray[index].Split(' ')[0]);
        index++;
        map = new int[num2, 8];
        floor = new int[num2][][,];
        for (int i = 0; i < num2; i++)
        {
            this.floor[i] = new int[2][,];
            char[] chArray3 = new char[] { ' ' };
            tmpstr = strArray[index].Split(chArray3);
            index++;
            for (int k = 0; k < 8; k++)
            {
                this.map[i, k] = int.Parse(tmpstr[k]);
            }
        }
        tmpstr = strArray[index].Split(' ');
        index++;
        roomid[0] = int.Parse(tmpstr[0]);
        roomid[1] = int.Parse(tmpstr[1]);
        for (int j = 0; j < num2; j++)
        {
            tmpstr = strArray[index].Split(' ');
            index++;
            int x = int.Parse(tmpstr[0]);
            int y = int.Parse(tmpstr[1]);
            for (int k = 0; k < 2; k++)
            {
                floor[j][k] = new int[x, y];
                for (int m = 0; m < y; m++)
                {
                    tmpstr = strArray[index].Split(' ');
                    index++;
                    for (int n = 0; n < x; n++)
                    {
                        if ((floor[j][k][n, m] = int.Parse(tmpstr[n])) == 12)
                        {
                            keys[0]++;
                        }
                    }
                }
            }
            if (path == "lastsave.ncs") file = strArray[index];
        }
        PrintRoom(roomid[0], roomid[1]);
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
        keys[0] -= keys[1];
        keys[1] = 0;
        int num = map[roomid[0], door] - 1;
        if (map[roomid[0], door] != 0)
        {
            SaveRoom();
            roomid[1] = map[roomid[0], door + 4];
            roomid[0] = num;
            PrintRoom(roomid[0], roomid[1]);
        }
        SaveGame();
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
        maxX = R[0].GetLength(0);
        maxY = R[0].GetLength(1);
        int[] numArray = FindDoor(pos);
        if (numArray != null)
        {
            int x = numArray[0];
            int y = numArray[1];
            ClearRoom();
            thisRoom[0] = new int[this.maxX, this.maxY];
            thisRoom[1] = new int[this.maxX, this.maxY];
            RObjects[0] = new GameObject[this.maxX, this.maxY];
            RObjects[1] = new GameObject[this.maxX, this.maxY];
            for (int i = 0; i < this.maxX; i++)
            {
                for (int j = 0; j < this.maxY; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        this.thisRoom[k][i, j] = R[k][i, j];
                        if (((R[k][i, j] > 0) && (R[k][i, j] <= this.objects.Length)) && (R[k][i, j] != 2))
                        {
                            GameObject obj2 = Instantiate(objects[R[k][i, j] - 1]);
                            obj2.GetComponent<CellPos>().SetPos(i, j, k * 0.5f);
                            this.RObjects[k][i, j] = obj2;
                        }
                    }
                }
            }
            objects[1].GetComponent<CellPos>().SetPos(x, y, 0.5f);
            objects[1].GetComponent<Rotation>().rotation = Rotation.Flip(pos);
            RObjects[1][x, y] = this.objects[1];
            SetCell(x, y, 2, 1);
        }
    }

    public void SaveGame(string path = "lastsave.ncs")
    {
        string str2;
        int length = floor.Length;
        string data = length.ToString() + " -|";
        for (int i = 0; i < length; i++)
        {
            str2 = "";
            for (int k = 0; k < 8; k++)
            {
                str2 += map[i, k].ToString();
                if (k < 7)
                {
                    str2 = str2 + " ";
                }
            }
            data = data + str2 + "|";
        }
        string[] textArray1 = new string[] { data, roomid[0].ToString(), " ", roomid[1].ToString(), "|" };
        data = string.Concat(textArray1);
        for (int j = 0; j < length; j++)
        {
            int num5 = this.floor[j][0].GetLength(0);
            int num6 = this.floor[j][0].GetLength(1);
            string[] textArray2 = new string[] { data, num5.ToString(), " ", num6.ToString(), "|" };
            data = string.Concat(textArray2);
            for (int k = 0; k < 2; k++)
            {
                for (int m = 0; m < num6; m++)
                {
                    str2 = "";
                    for (int n = 0; n < num5; n++)
                    {
                        str2 += floor[j][k][n, m];
                        if (n < (num5 - 1))
                        {
                            str2 = str2 + " ";
                        }
                    }
                    data = data + str2 + "|";
                }
            }
        }
        if (path == "lastsave.ncs") data += file;
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
                        floor[roomid[0]][i][k, j] = thisRoom[i][k, j];
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
            GameObject obj2 = Instantiate(objects[v - 1]);
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