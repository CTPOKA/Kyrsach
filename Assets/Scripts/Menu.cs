using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public bool esc;
    public GameObject dlist;

    public void Close()
    {
        Application.Quit();
    }

    public void Init()
    {
        try
        {
            List<string> list = new List<string>();
            foreach (string str in Directory.GetFiles(Application.persistentDataPath + "/Floors", "*.ncs"))
            {
                string str2;
                if (Crypt.Check(str))
                {
                    str2 = "✔ ";
                }
                else
                {
                    str2 = "";
                }
                char[] separator = new char[] { '\\' };
                char[] chArray2 = new char[] { '.' };
                str2 = str2 + str.Split(separator)[1].Split(chArray2)[0];
                list.Add(str2);
            }
            dlist.GetComponent<Dropdown>().ClearOptions();
            dlist.GetComponent<Dropdown>().AddOptions(list);
        }
        catch
        {
            Error.ErrorMassage("Ошибка чтения файлов. Проверьте наличие и целостность содержимого папки " + Application.persistentDataPath + "/Floors");
        }
    }

    public void LoadGame(string s)
    {
        SceneManager.LoadScene("main");
        RoomSpace.file = s;
    }

    public void LoadNewGame()
    {
        string text = this.dlist.GetComponent<Dropdown>().captionText.text;
        if (text.IndexOf("✔") >= 0)
        {
            text = text.Remove(0, 2);
        }
        this.LoadGame("Floors/" + text + ".ncs");
    }

    public void LoadScene(string s)
    {
        SceneManager.LoadScene(s);
    }

    public void RemoveGame()
    {
        try
        {
            string text = dlist.GetComponent<Dropdown>().captionText.text;
            if (text.IndexOf("✔") >= 0)
            {
                text = text.Remove(0, 2);
            }
            File.Delete(Application.persistentDataPath + "/Floors/" + text + ".ncs");
            Init();
        }
        catch
        {
            Debug.Log("Ошибка удаления");
        }
    }

    private void Update()
    {
        Player player;
        if ((this.esc && (RoomSpace.M.objects[1] != null)) && ((player = RoomSpace.M.objects[1].GetComponent<Player>()) != null))
        {
            if (Input.GetKeyDown("escape"))
            {
                if (base.GetComponent<Canvas>().enabled)
                {
                    base.GetComponent<Canvas>().enabled = false;
                }
                else
                {
                    base.GetComponent<Canvas>().enabled = true;
                    player.enabled = false;
                }
            }
            if (!base.GetComponent<Canvas>().enabled)
            {
                player.enabled = true;
            }
        }
        else
        {
            this.esc = false;
        }
    }
}
