using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class FloorEditor : MonoBehaviour
{
    public GameObject save;
    public GameObject room;
    List<GameObject> rooms = new List<GameObject>();
    static public List<string> names = new List<string>();
    static public List<string[]> map = new List<string[]>();
    public void Init()
    {
        try
        {
            List<string> list = new List<string>();
            foreach (var r in Directory.GetFiles(Application.persistentDataPath + "/Rooms", "*.ncs"))
            {
                list.Add(r.Split('\\')[1].Split('.')[0]);
            }
            GetComponent<Dropdown>().ClearOptions();
            GetComponent<Dropdown>().AddOptions(list);
        }
        catch { Error.ErrorMassage("Ошибка чтения файлов. Проверьте наличие папки Game_Data" + "/Rooms"); }
    }

    public void AddRoom()
    {
        string str = GetComponent<Dropdown>().captionText.text;
        if (names.IndexOf(str) == -1)
        {
            GameObject obj = Instantiate(room) as GameObject;
            obj.transform.SetParent(transform.parent);
            obj.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position + 100 * Vector3.up;
            obj.GetComponentInChildren<Text>().text = str;
            names.Add(str);
            rooms.Add(obj);
            map.Add(new string[8]);
        }
    }
    public void RemoveRoom(GameObject list)
    {
        int i = names.IndexOf(list.GetComponent<Dropdown>().captionText.text);
        if (i >= 0)
        {
            Destroy(rooms[i]);
            rooms.RemoveAt(i);
            names.RemoveAt(i);
            map.RemoveAt(i);
        }
    }

    public void SaveFloor()
    {
        string tmp = "";
        string str;
        int rooms = names.Count;
        tmp+=rooms.ToString()+" -|";
        for (int i = 0; i < rooms; i++)
        {
            str = "";
            for (int j = 0; j < 8; j++)
            {
                string m;
                if (j < 4)
                {
                    m = (names.IndexOf(map[i][j]) + 1).ToString();
                }
                else
                {
                    m = map[i][j];
                    if (m == "" || m == null) m = "0";
                }
                str +=m;
                if (j<7) str += " ";
            }
            tmp+=str+"|";
        }
        tmp+=names.IndexOf(Node.spawn[0]) +" "+ Node.spawn[1] + "|";
        for (int i = 0; i < rooms; i++)
        {
            tmp += Crypt.Read("Rooms/" + names[i] + ".ncs");
        }
        Crypt.Write("Floors/" + save.GetComponent<InputField>().text + ".ncs", tmp);
    }
}
