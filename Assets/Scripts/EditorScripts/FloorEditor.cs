using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class FloorEditor : MonoBehaviour
{
    public GameObject save;

    public GameObject room;

    private List<GameObject> rooms = new List<GameObject>();

    public static List<string> names = new List<string>();

    public static List<string[]> map = new List<string[]>();

    public void Init()
    {
        try
        {
            List<string> list = new List<string>();
            string[] files = Directory.GetFiles(Application.persistentDataPath + "/Rooms", "*.ncs");
            for (int i = 0; i < files.Length; i++)
            {
                string text = files[i];
                list.Add(text.Split(new char[]
                {
                    '\\'
                })[1].Split(new char[]
                {
                    '.'
                })[0]);
            }
            base.GetComponent<Dropdown>().ClearOptions();
            base.GetComponent<Dropdown>().AddOptions(list);
        }
        catch
        {
            Error.ErrorMassage("Ошибка чтения файлов. Проверьте наличие папки Game_Data/Rooms");
        }
    }

    public void AddRoom()
    {
        string text = base.GetComponent<Dropdown>().captionText.text;
        if (names.IndexOf(text) == -1 && text != "")
        {
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.room);
            gameObject.transform.SetParent(base.transform.parent);
            gameObject.GetComponent<RectTransform>().position = base.GetComponent<RectTransform>().position + 100f * Vector3.up;
            gameObject.GetComponentInChildren<Text>().text = text;
            FloorEditor.names.Add(text);
            this.rooms.Add(gameObject);
            FloorEditor.map.Add(new string[8]);
        }
    }

    public void RemoveRoom(GameObject list)
    {
        int num = FloorEditor.names.IndexOf(list.GetComponent<Dropdown>().captionText.text);
        if (num >= 0)
        {
            UnityEngine.Object.Destroy(this.rooms[num]);
            this.rooms.RemoveAt(num);
            FloorEditor.names.RemoveAt(num);
            FloorEditor.map.RemoveAt(num);
        }
    }

    public void SaveFloor()
    {
        string text = "";
        int count = FloorEditor.names.Count;
        text = text + count.ToString() + " -|";
        for (int i = 0; i < count; i++)
        {
            string text2 = "";
            for (int j = 0; j < 8; j++)
            {
                string text3;
                if (j < 4)
                {
                    text3 = (FloorEditor.names.IndexOf(FloorEditor.map[i][j]) + 1).ToString();
                }
                else
                {
                    text3 = FloorEditor.map[i][j];
                    if (text3 == "" || text3 == null)
                    {
                        text3 = "0";
                    }
                }
                text2 += text3;
                if (j < 7)
                {
                    text2 += " ";
                }
            }
            text = text + text2 + "|";
        }
        text = string.Concat(new object[]
        {
            text,
            FloorEditor.names.IndexOf(Node.spawn[0]),
            " ",
            Node.spawn[1],
            "|"
        });
        for (int k = 0; k < count; k++)
        {
            text += Crypt.Read("Rooms/" + FloorEditor.names[k] + ".ncs");
        }
        Crypt.Write("Floors/" + save.GetComponent<InputField>().text + ".ncs", text);
    }
}
