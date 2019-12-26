using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class FileList : MonoBehaviour
{
    List<string> path = new List<string>();
    public const string newroom = "*Новая комната*";
    public static Dropdown caption;
    private void Awake()
    {
        caption = GetComponent<Dropdown>();
        Init();
    }
    void Init ()
    {
        try
        {
            List<string> list = new List<string>();
            list.Add(newroom);
            path.Clear();
            path.Add("");
            foreach (var r in Directory.GetFiles(Application.persistentDataPath + "/Rooms", "*.ncs"))
            {
                path.Add(r);
                list.Add(r.Split('\\')[1].Split('.')[0]);
            }
            GetComponent<Dropdown>().ClearOptions();
            GetComponent<Dropdown>().AddOptions(list);
        }
        catch
        {
            Error.ErrorMassage("Ошибка чтения файлов. Проверьте наличие папки Game_Data" + Application.persistentDataPath + "/Rooms");
        }
    }
    public void Load(GameObject list)
    {
        string str = list.GetComponent<Dropdown>().captionText.text;
        if (str != newroom)
        {
            Editor.E.LoadRoom(str + ".ncs");
        }
    }
    public void Save()
    {
        string str = GetComponent<Dropdown>().captionText.text;
        if (str != newroom && str != "" && str != null)
        {
            Editor.E.SaveRoom(str + ".ncs");
        }
        else
        {
            GameObject.Find("SaveRoom").GetComponent<Canvas>().enabled = true;
        }
    }
    public void NewRoom(GameObject inputname)
    {
        string str = inputname.GetComponent<InputField>().text;
        if (str!="") Editor.E.SaveRoom(str + ".ncs");
        Init();
    }
    public void Delete()
    {
        int val = GetComponent<Dropdown>().value;
        if (val > 0)
        {
            System.IO.File.Delete(path[val]);
            System.IO.File.Delete(val + ".meta");
            path.Remove(path[val]);
            Init();
        }
    }
}
