using System.Collections;
using UnityEngine;
public class RoomSpace : MonoBehaviour
{
    //Глобальные константы
    public const float cellsizeX = 1.15f;
    public const float cellsizeY = 0.78f;

    public GameObject[] objects;
    GameObject[][,] RObjects = new GameObject[2][,];
    public int[][][,] floor;
    public int[][,] thisRoom = new int[2][,];
    public int tasks;
    public int[] keys = new int[2];
    public int[,] map;
    public int maxX, maxY;
    public static int[] roomid = new int[2];
    static public RoomSpace M;
    static public string file;

    static public BitArray intangible;
    static public BitArray bigobject;
    static public BitArray movable;
    static public BitArray killing;
    static public BitArray door;
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

        try { LoadFloor(file); }
        catch { Error.ErrorMassage("Ошибка чтения файла. Возможно он был поврежден"); }

    }
    public void PrintRoom(int room, int pos)
    {
        int[][,] R = floor[room];
        maxX = R[0].GetLength(0);
        maxY = R[0].GetLength(1);
        int[] pp = FindDoor(pos);
        if (pp == null) return;
        int px = pp[0], py = pp[1];
        ClearRoom();
        thisRoom[0] = new int[maxX, maxY];
        thisRoom[1] = new int[maxX, maxY];
        RObjects[0] = new GameObject[maxX, maxY];
        RObjects[1] = new GameObject[maxX, maxY];
        for (int i = 0; i < maxX; i++)
        {
            for (int j = 0; j < maxY; j++)
            {
                for (int l = 0; l < 2; l++)
                {
                    thisRoom[l][i, j] = R[l][i, j];
                    if (R[l][i, j] > 0 && R[l][i, j] <= objects.Length)
                    {
                        if (R[l][i, j] != 2)
                        {
                            GameObject obj = Instantiate(objects[R[l][i, j] - 1]) as GameObject;
                            obj.GetComponent<CellPos>().SetPos(i, j, l * 0.5f);
                            RObjects[l][i, j] = obj;
                        }
                    }
                }
            }
        }
        objects[1].GetComponent<CellPos>().SetPos(px, py, 0.5f);
        objects[1].GetComponent<Rotation>().rotation = Rotation.Flip(pos);
        RObjects[1][px, py] = objects[1];
        SetCell(px, py, 2);
        void ClearRoom()
        {
            tasks = 0;
            for (int i = 0; i < RObjects[0].GetLength(0); i++)
            {
                for (int j = 0; j < RObjects[0].GetLength(1); j++)
                {
                    for (int l = 0; l < 2; l++)
                    {
                        if (RObjects[l][i, j] != objects[1])
                        {
                            Destroy(RObjects[l][i, j]);
                        }
                    }
                }
            }
        }
        int[] FindDoor(int id)
        {
            int[] p = new int[2];
            p[0] = -1;
            p[1] = -1;
            switch (id)
            {
                case (Rotation.Up):
                    for (int i = 0; i < maxX; i++)
                    {
                        if (door[R[0][i, maxY - 1]])
                            p[0] = i;
                    }
                    p[1] = maxY - 1;
                    break;
                case (Rotation.Down):
                    for (int i = 0; i < maxX; i++)
                    {
                        if (door[R[0][i, 0]])
                            p[0] = i;
                    }
                    p[1] = 0;
                    break;
                case (Rotation.Left):
                    for (int i = 0; i < maxY; i++)
                    {
                        if (door[R[0][0, i]])
                            p[1] = i;
                    }
                    p[0] = 0;
                    break;
                case (Rotation.Right):
                    for (int i = 0; i < maxY; i++)
                    {
                        if (door[R[0][maxX - 1, i]])
                            p[1] = i;
                    }
                    p[0] = maxX - 1;
                    break;
            }
            if (p[0] >= 0 && p[1] >= 0 && intangible[R[1][p[0], p[1]]])
                return p;
            else return null;
        }
    }
    public int Move(int x, int y, int dx, int dy, int level = 1)
    {
        if (x + dx >= 0 && y + dy >= 0 && x + dx < thisRoom[0].GetLength(0) && y + dy < thisRoom[0].GetLength(1))
        {
            if (GetCell(x+dx,y+dy,level)>=0 && intangible[thisRoom[level][x + dx, y + dy]] && GetCell(x + dx, y + dy, 0) > 0)
            {
                RObjects[level][x, y].GetComponent<CellPos>().Move(dx, dy);
                RObjects[level][x + dx, y + dy] = RObjects[level][x, y];
                RObjects[level][x, y] = null;
                thisRoom[level][x + dx, y + dy] = thisRoom[level][x, y];
                thisRoom[level][x, y] = 0;
            }
            else return 1;
            return 0;
        }
        else return 2;
    }
    public int GetCell(int x, int y, int level = 1)
    {
        if (x >= 0 && y >= 0 && x < thisRoom[0].GetLength(0) && y < thisRoom[0].GetLength(1))
        {
            return thisRoom[level][x, y];
        }
        return -1;
    }
    public GameObject GetObject(int x, int y, int level = 1)
    {
        if (x >= 0 && y >= 0 && x < thisRoom[level].GetLength(0) && y < thisRoom[level].GetLength(1) && GetCell(x,y,level)!=0)
        {
            return RObjects[level][x, y];
        }
        return null;
    }
    public void SetCell(int x, int y, int v, int level = 1)
    {
        thisRoom[level][x, y] = v;
    }
    public GameObject SetObject(int x, int y, int v, int level = 1)
    {
        Destroy(RObjects[level][x, y]);
        thisRoom[level][x, y] = v;
        if (v > 0 && v <= objects.Length)
        {
            GameObject obj = Instantiate(objects[v - 1]) as GameObject;
            obj.GetComponent<CellPos>().SetPos(x, y, level * 0.5f);
            RObjects[level][x, y] = obj;
            return obj;
        }
        return null;
    }
    public void LoadFloor(string path = "lastsave.ncs")
    {
        keys[0] = 0;
        keys[1] = 0;
        if (path == "") path = file;
        string[] tmp = Crypt.Read(path).Split('|');
        int n = 0;
        int rooms = int.Parse(tmp[n].Split(' ')[0]); n++;
        map = new int[rooms, 8];
        string[] str;
        floor = new int[rooms][][,];
        for (int i = 0; i < rooms; i++)
        {
            floor[i] = new int[2][,];
            str = tmp[n].Split(' '); n++;
            for (int j = 0; j < 8; j++)
            {
                map[i, j] = int.Parse(str[j]);
            }
        }
        str = tmp[n].Split(' '); n++;
        roomid[0] = int.Parse(str[0]);
        roomid[1] = int.Parse(str[1]);
        for (int i = 0; i < rooms; i++)
        {
            str = tmp[n].Split(' '); n++;
            int x = int.Parse(str[0]), y = int.Parse(str[1]);
            for (int j = 0; j<2; j++)
            {
                floor[i][j] = new int[x, y];
                for (int yi = 0; yi < y; yi++)
                {
                    str = tmp[n].Split(' '); n++;
                    for (int xi = 0; xi < x; xi++)
                    {
                        if ((floor[i][j][xi, yi] = int.Parse(str[xi])) == 12) keys[0]++;
                    }
                }
            }
        }
        PrintRoom(roomid[0], roomid[1]);
    }
    public void SaveRoom()
    {
        int x = maxX, y = maxY;
        for (int j = 0; j < 2; j++)
        {
            for (int yi = 0; yi < y; yi++)
            {
                for (int xi = 0; xi < x; xi++)
                {
                    if (thisRoom[j][xi,yi]!=2)
                    {
                        floor[roomid[0]][j][xi, yi] = thisRoom[j][xi, yi];
                    }
                }
            }
        }
    }
    public void NextRoom(int door)
    {
        keys[0] -= keys[1];
        keys[1] = 0;
        int id = map[roomid[0],door]-1;
        if (map[roomid[0],door]!=0)
        {
            SaveRoom();
            roomid[1] = map[roomid[0], door + 4];
            roomid[0] = id;
            PrintRoom(roomid[0], roomid[1]);
        }
    }
    public void SaveGame(string path = "lastsave.ncs")
    {
        string tmp = "";
        int rooms = floor.Length;
        string str;
        tmp+=rooms.ToString()+" -|";
        for (int i = 0; i < rooms; i++)
        {
            str = "";
            for (int j = 0; j < 8; j++)
            {
                str += map[i, j].ToString();
                if (j < 7) str += " ";
            }
            tmp+=str+"|";
        }
        tmp+=roomid[0].ToString()+" "+roomid[1].ToString()+"|";
        for (int i = 0; i < rooms; i++)
        {
            int x = floor[i][0].GetLength(0), y = floor[i][0].GetLength(1);
            tmp+=x.ToString() + " " + y.ToString()+"|";
            for (int j = 0; j < 2; j++)
            {
                for (int yi = 0; yi < y; yi++)
                {
                    str = "";
                    for (int xi = 0; xi < x; xi++)
                    {
                        str += floor[i][j][xi, yi];
                        if (xi < x-1) str += " ";
                    }
                    tmp+=str+"|";
                }
            }
        }
        Crypt.Write(path, tmp);
    }
    public void Victory()
    {
        string str = Crypt.Read(file);
        Crypt.Write(file, str.Replace("-","+"));
    }
}
