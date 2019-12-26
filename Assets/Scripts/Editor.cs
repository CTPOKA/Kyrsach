using System.Collections;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
public class Editor : MonoBehaviour
{
    public const float cellsizeX = 1.15f;
    public const float cellsizeY = 0.78f;
    public const int saves = 20;

    public int[][,] thisroom = new int[2][,];
    int[][][,] backup = new int[saves][][,];
    public GameObject cell;
    public GameObject[][,] objects;
    public Sprite[] sprites;
    static public Editor E;
    public int X, Y, bn, bmin, bmax;
    public int type = 1, lvl = 0;
    public BitArray OFF = new BitArray(5);
    void Awake()
    {

        E = this;

        thisroom[0] = new int[X, Y];
        thisroom[1] = new int[X, Y];
        for (int x = 0; x < X; x++)
        {
            for (int y = 0; y < Y; y++)
            {
                GameObject obj = Instantiate(cell) as GameObject;
                obj.GetComponent<Cell>().CellInit(x, y, 0);
                obj = Instantiate(cell) as GameObject;
                obj.GetComponent<Cell>().CellInit(x, y, 1);
            }
        }
        SaveBackup();
        LoadRoom();
    }
    public void SetBrush (int t)
    {
        type = t;
    }
    public void SetLevel(int l)
    {
        lvl = l;
    }

    public void SaveBackup()
    {
        bn++;
        int[][,] temp = new int[2][,];
        temp[0] = new int[X, Y];
        temp[1] = new int[X, Y];
        for (int l = 0; l<2; l++)
        {
            for (int x = 0; x < X; x++)
            {
                for (int y = 0; y < Y; y++)
                {
                    temp[l][x, y] = thisroom[l][x, y];
                }
            }
        }
        backup[bn%saves]=temp;
        if (bn - saves > bmin) bmin++;
        bmax = bn;
    }
    void MoveRoom(int dx, int dy)
    {
        for (int l = 0; l < 2; l++)
        {
            for (int x = 0; x < X; x++)
            {
                for (int y = 0; y < Y; y++)
                {
                    if (x - dx >= 0 && x - dx < X && y - dy >= 0 && y - dy < Y)
                        thisroom[l][x, y] = backup[bn % saves][l][x - dx, y - dy];
                    else thisroom[l][x, y] = 0;
                }
            }
        }
        SaveBackup();
    }
    public void LoadBackup()
    {
        for (int l = 0; l < 2; l++)
        {
            for (int x = 0; x < X; x++)
            {
                for (int y = 0; y < Y; y++)
                {
                    thisroom[l][x, y] = backup[bn % saves][l][x, y];
                }
            }
        }
    }

    int[] Borders()
    {
        int xmin = X, xmax = 0, ymin = Y, ymax = 0;
        for (int x = 0; x < X; x++)
        {
            for (int y = 0; y < Y; y++)
            {
                for (int l = 0; l < 2; l++)
                {
                    if (thisroom[l][x, y] != 0)
                    {
                        if (x < xmin) xmin = x;
                        if (x > xmax) xmax = x;
                        if (y < ymin) ymin = y;
                        if (y > ymax) ymax = y;
                    }
                }
            }
        }
        return new int[4] { xmin, xmax, ymin, ymax };
    }
    public void LoadRoom(string path = "backup.ncs")
    {
        try
        {
            string file = Application.persistentDataPath + "/Rooms/" + path;
            Clear();
            if (!File.Exists(file)) return;
            string[] tmp = Crypt.Read("/Rooms/" + path).Split('|');
            string[] str;
            int n = 0;
            str = tmp[n].Split(' '); n++;
            int x = int.Parse(str[0]), y = int.Parse(str[1]);
            int dx = (X - x) / 2;
            int dy = (Y - y) / 2;
            for (int j = 0; j < 2; j++)
            {
                for (int yi = 0; yi < y; yi++)
                {
                    str = tmp[n].Split(' '); n++;
                    for (int xi = 0; xi < x; xi++)
                    {
                        thisroom[j][xi + dx, yi + dy] = int.Parse(str[xi]);
                    }
                }
            }
            SaveBackup();
        }
        catch { Error.ErrorMassage("Ошибка чтения файла. Возможно он был поврежден"); }
    }
    public void SaveRoom(string path = "backup.ncs")
    {
        string tmp = "";
        string str;
        int[] b = Borders();
        int xmin = b[0], xmax = b[1]+1, ymin = b[2], ymax = b[3]+1;
        tmp+=(xmax-xmin).ToString() + " " + (ymax-ymin).ToString() + "|";
        for (int j = 0; j < 2; j++)
        {
            for (int yi = ymin; yi < ymax; yi++)
            {
                str = "";
                for (int xi = xmin; xi < xmax; xi++)
                {
                    str += thisroom[j][xi, yi];
                    if (xi < xmax-1) str += " ";
                }
                tmp+=str + "|";
            }
        }
        Crypt.Write("/Rooms/" + path,tmp);
    }
    private void Update()
    {
        if (GameObject.Find("SaveRoom").GetComponent<Canvas>().enabled==false && GameObject.Find("SaveFloat").GetComponent<Canvas>().enabled == false)
        {
            if (Input.GetKeyDown(KeyCode.Z) && Input.GetKey(KeyCode.LeftControl))
            {
                if (bn > bmin + 1)
                {
                    bn--;
                    LoadBackup();
                }
            }
            if (Input.GetKeyDown(KeyCode.Y) && Input.GetKey(KeyCode.LeftControl))
            {
                if (bn < bmax)
                {
                    bn++;
                    LoadBackup();
                }
            }
            if (Input.GetKeyDown("up") && Borders()[3] < Y - 1)
            {
                MoveRoom(0, 1);
            }
            else if (Input.GetKeyDown("down") && Borders()[2] > 0)
            {
                MoveRoom(0, -1);
            }
            else if (Input.GetKeyDown("right") && Borders()[1] < X - 1)
            {
                MoveRoom(1, 0);
            }
            else if (Input.GetKeyDown("left") && Borders()[0] > 0)
            {
                MoveRoom(-1, 0);
            }
            if (Input.GetKeyDown("escape"))
            {
                string save = FileList.newroom;
                if (FileList.caption!=null) save = FileList.caption.captionText.text;
                if ( save == FileList.newroom) SaveRoom();
                else SaveRoom(save + ".ncs");
                FloorEditor.names.Clear();
                FloorEditor.map.Clear();
                SceneManager.LoadScene("menu");
            }
            if (Input.GetKeyDown(KeyCode.R) || (Input.GetKeyDown(KeyCode.Delete)))
            {
                Clear();
                SaveBackup();
            }
        }
    }
    void Clear ()
    {
        for (int x = 0; x < X; x++)
        {
            for (int y = 0; y < Y; y++)
            {
                thisroom[0][x, y] = 0;
                thisroom[1][x, y] = 0;
            }
        }
    }
    public void TriggerOFF()
    {
        OFF[1] = !OFF[1];
    }
}
