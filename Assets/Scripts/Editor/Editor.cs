using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Editor : MonoBehaviour
{
    public const float cellsizeX = 1.15f;

    public const float cellsizeY = 0.78f;

    public const int saves = 20;

    public int[][,] thisroom = new int[2][,];

    private int[][][,] backup = new int[20][][,];

    public GameObject cell;

    public GameObject[][,] objects;

    public Sprite[] sprites;

    public static Editor E;

    public int X;

    public int Y;

    public int bn;

    public int bmin;

    public int bmax;

    public int type = 1;

    public int lvl;

    public BitArray OFF = new BitArray(5);

    private void Awake()
    {
        Editor.E = this;
        this.thisroom[0] = new int[this.X, this.Y];
        this.thisroom[1] = new int[this.X, this.Y];
        for (int i = 0; i < this.X; i++)
        {
            for (int j = 0; j < this.Y; j++)
            {
                UnityEngine.Object.Instantiate<GameObject>(this.cell).GetComponent<Cell>().CellInit(i, j, 0);
                UnityEngine.Object.Instantiate<GameObject>(this.cell).GetComponent<Cell>().CellInit(i, j, 1);
            }
        }
        this.SaveBackup();
        this.LoadRoom("backup.ncs");
    }

    public void SetBrush(int t)
    {
        this.type = t;
    }

    public void SetLevel(int l)
    {
        this.lvl = l;
    }

    public void SaveBackup()
    {
        this.bn++;
        int[][,] array = new int[][,]
        {
            new int[this.X, this.Y],
            new int[this.X, this.Y]
        };
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < this.X; j++)
            {
                for (int k = 0; k < this.Y; k++)
                {
                    array[i][j, k] = this.thisroom[i][j, k];
                }
            }
        }
        this.backup[this.bn % 20] = array;
        if (this.bn - 20 > this.bmin)
        {
            this.bmin++;
        }
        this.bmax = this.bn;
    }

    private void MoveRoom(int dx, int dy)
    {
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < this.X; j++)
            {
                for (int k = 0; k < this.Y; k++)
                {
                    if (j - dx >= 0 && j - dx < this.X && k - dy >= 0 && k - dy < this.Y)
                    {
                        this.thisroom[i][j, k] = this.backup[this.bn % 20][i][j - dx, k - dy];
                    }
                    else
                    {
                        this.thisroom[i][j, k] = 0;
                    }
                }
            }
        }
        this.SaveBackup();
    }

    public void LoadBackup()
    {
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < this.X; j++)
            {
                for (int k = 0; k < this.Y; k++)
                {
                    this.thisroom[i][j, k] = this.backup[this.bn % 20][i][j, k];
                }
            }
        }
    }

    private int[] Borders()
    {
        int num = this.X;
        int num2 = 0;
        int num3 = this.Y;
        int num4 = 0;
        for (int i = 0; i < this.X; i++)
        {
            for (int j = 0; j < this.Y; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    if (this.thisroom[k][i, j] != 0)
                    {
                        if (i < num)
                        {
                            num = i;
                        }
                        if (i > num2)
                        {
                            num2 = i;
                        }
                        if (j < num3)
                        {
                            num3 = j;
                        }
                        if (j > num4)
                        {
                            num4 = j;
                        }
                    }
                }
            }
        }
        return new int[]
        {
            num,
            num2,
            num3,
            num4
        };
    }

    public void LoadRoom(string path = "backup.ncs")
    {
        try
        {
            string arg_16_0 = Application.persistentDataPath + "/Rooms/" + path;
            this.Clear();
            if (File.Exists(arg_16_0))
            {
                string[] array = Crypt.Read("/Rooms/" + path).Split(new char[]
                {
                    '|'
                });
                int num = 0;
                string[] array2 = array[num].Split(new char[]
                {
                    ' '
                });
                num++;
                int num2 = int.Parse(array2[0]);
                int num3 = int.Parse(array2[1]);
                int num4 = (this.X - num2) / 2;
                int num5 = (this.Y - num3) / 2;
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < num3; j++)
                    {
                        array2 = array[num].Split(new char[]
                        {
                            ' '
                        });
                        num++;
                        for (int k = 0; k < num2; k++)
                        {
                            this.thisroom[i][k + num4, j + num5] = int.Parse(array2[k]);
                        }
                    }
                }
                this.SaveBackup();
            }
        }
        catch
        {
            Error.ErrorMassage("Ошибка чтения файла. Возможно он был поврежден");
        }
    }

    public void SaveRoom(string path = "backup.ncs")
    {
        string text = "";
        int[] expr_0C = this.Borders();
        int num = expr_0C[0];
        int num2 = expr_0C[1] + 1;
        int num3 = expr_0C[2];
        int num4 = expr_0C[3] + 1;
        text = string.Concat(new string[]
        {
            text,
            (num2 - num).ToString(),
            " ",
            (num4 - num3).ToString(),
            "|"
        });
        for (int i = 0; i < 2; i++)
        {
            for (int j = num3; j < num4; j++)
            {
                string text2 = "";
                for (int k = num; k < num2; k++)
                {
                    text2 += this.thisroom[i][k, j];
                    if (k < num2 - 1)
                    {
                        text2 += " ";
                    }
                }
                text = text + text2 + "|";
            }
        }
        Crypt.Write("/Rooms/" + path, text);
    }

    private void Update()
    {
        if (!GameObject.Find("SaveRoom").GetComponent<Canvas>().enabled && !GameObject.Find("SaveFloat").GetComponent<Canvas>().enabled)
        {
            if (Input.GetKeyDown(KeyCode.Z) && Input.GetKey(KeyCode.LeftControl) && this.bn > this.bmin + 1)
            {
                this.bn--;
                this.LoadBackup();
            }
            if (Input.GetKeyDown(KeyCode.Y) && Input.GetKey(KeyCode.LeftControl) && this.bn < this.bmax)
            {
                this.bn++;
                this.LoadBackup();
            }
            if (Input.GetKeyDown("up") && this.Borders()[3] < this.Y - 1)
            {
                this.MoveRoom(0, 1);
            }
            else if (Input.GetKeyDown("down") && this.Borders()[2] > 0)
            {
                this.MoveRoom(0, -1);
            }
            else if (Input.GetKeyDown("right") && this.Borders()[1] < this.X - 1)
            {
                this.MoveRoom(1, 0);
            }
            else if (Input.GetKeyDown("left") && this.Borders()[0] > 0)
            {
                if (base.GetComponent<AudioSource>().isPlaying)
                {
                    MonoBehaviour.print(1);
                }
                this.MoveRoom(-1, 0);
            }
            if (Input.GetKeyDown("escape"))
            {
                string text = "*Новая комната*";
                if (FileList.caption != null)
                {
                    text = FileList.caption.captionText.text;
                }
                if (text == "*Новая комната*")
                {
                    this.SaveRoom("backup.ncs");
                }
                else
                {
                    this.SaveRoom(text + ".ncs");
                }
                FloorEditor.names.Clear();
                FloorEditor.map.Clear();
                SceneManager.LoadScene("menu");
            }
            if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Delete))
            {
                this.Clear();
                this.SaveBackup();
            }
        }
    }

    private void Clear()
    {
        for (int i = 0; i < this.X; i++)
        {
            for (int j = 0; j < this.Y; j++)
            {
                this.thisroom[0][i, j] = 0;
                this.thisroom[1][i, j] = 0;
            }
        }
    }

    public void TriggerOFF()
    {
        this.OFF[1] = !this.OFF[1];
    }
}