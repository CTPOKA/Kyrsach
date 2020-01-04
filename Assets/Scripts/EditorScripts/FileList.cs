using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class FileList : MonoBehaviour
{
    private List<string> path = new List<string>();

    public const string newroom = "*Новая комната*";

    public static Dropdown caption;

    public GameObject flist;

    private void Awake()
    {
        caption = GetComponent<Dropdown>();
        Init(Editor.file);
    }

    private void Init(string caption = newroom)
    {
        flist.GetComponent<FloorEditor>().Init();
        try
        {
            List<string> list = new List<string>();
            list.Add("*Новая комната*");
            this.path.Clear();
            this.path.Add("");
            string[] files = Directory.GetFiles(Application.persistentDataPath + "/Rooms", "*.ncs");
            for (int i = 0; i < files.Length; i++)
            {
                string text = files[i];
                this.path.Add(text);
                list.Add(text.Split(new char[]
                {
                    '\\'
                })[1].Split(new char[]
                {
                    '.'
                })[0]);
            }
            GetComponent<Dropdown>().ClearOptions();
            GetComponent<Dropdown>().AddOptions(list);
            GetComponent<Dropdown>().value = list.IndexOf(caption);
        }
        catch
        {
            Error.ErrorMassage("Ошибка чтения файлов. Проверьте наличие папки Game_Data" + Application.persistentDataPath + "/Rooms");
        }
    }

    public void Load(GameObject list)
    {
        string text = list.GetComponent<Dropdown>().captionText.text;
        if (text != "*Новая комната*")
        {
            Editor.E.LoadRoom(text + ".ncs");
        }
    }

    public void Save()
    {
        string text = base.GetComponent<Dropdown>().captionText.text;
        if (text != "*Новая комната*" && text != "" && text != null)
        {
            Editor.E.SaveRoom(text + ".ncs");
            return;
        }
        GameObject.Find("SaveRoom").GetComponent<Canvas>().enabled = true;
    }

    public void NewRoom(GameObject inputname)
    {
        string text = inputname.GetComponent<InputField>().text;
        if (text != "")
        {
            Editor.E.SaveRoom(text + ".ncs");
            Init(text);
        }
    }

    public void Delete()
    {
        int value = base.GetComponent<Dropdown>().value;
        if (value > 0)
        {
            File.Delete(this.path[value]);
            File.Delete(value + ".meta");
            this.path.Remove(this.path[value]);
            this.Init();
        }
    }
}