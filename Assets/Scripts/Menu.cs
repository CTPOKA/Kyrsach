using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public bool esc;
    public GameObject dlist;

    public void LoadScene(string s)
    {
        SceneManager.LoadScene(s);
    }

    public void LoadGame(string s)
    {
        SceneManager.LoadScene("main");
        RoomSpace.file = s;
    }
    public void Close()
    {
        Application.Quit();
    }
    private void Update()
    {
        Player p;
        if (esc && RoomSpace.M.objects[1] != null && (p = RoomSpace.M.objects[1].GetComponent<Player>())!=null)
        {
            if (Input.GetKeyDown("escape"))
            {
                if (GetComponent<Canvas>().enabled == true)
                {
                    GetComponent<Canvas>().enabled = false;
                }
                else
                {
                    GetComponent<Canvas>().enabled = true;
                    p.enabled = false;
                }
            }
            if (GetComponent<Canvas>().enabled == false) p.enabled = true;
        }
        else esc = false;
    }
    public void Init()
    {
        try
        {
            List<string> list = new List<string>();
            foreach (var r in Directory.GetFiles(Application.persistentDataPath + "/Floors", "*.ncs"))
            {
                string str;
                if (Crypt.Check(r)) str = "✔ ";
                else str = "";
                str += r.Split('\\')[1].Split('.')[0];
                list.Add(str);
            }
            dlist.GetComponent<Dropdown>().ClearOptions();
            dlist.GetComponent<Dropdown>().AddOptions(list);
        }
        catch
        {
            Error.ErrorMassage("Ошибка чтения файлов. Проверьте наличие и целостность содержимого папки"+ Application.persistentDataPath + "/Floors");
        }
    }
    public void LoadNewGame()
    {
        string file = dlist.GetComponent<Dropdown>().captionText.text;
        if (file.IndexOf("✔") >= 0) file = file.Remove(0, 2);
        LoadGame("Floors/" + file + ".ncs");
    }

    public void RemoveGame()
    {
        string file="";
        try
        {
            file = dlist.GetComponent<Dropdown>().captionText.text;
            if (file.IndexOf("✔") >= 0) file = file.Remove(0, 2);
            file = Application.persistentDataPath + "/Floors/" + file + ".ncs";
            File.Delete(file);
            Init();
        }
        catch
        {
            Debug.Log("Ошибка удаления"+ file);
        }
    }
}
