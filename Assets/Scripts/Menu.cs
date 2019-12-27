using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        Player component;
        if (this.esc && RoomSpace.M.objects[1] != null && (component = RoomSpace.M.objects[1].GetComponent<Player>()) != null)
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
                    component.enabled = false;
                }
            }
            if (!base.GetComponent<Canvas>().enabled)
            {
                component.enabled = true;
                return;
            }
        }
        else
        {
            this.esc = false;
        }
    }

    public void Init()
    {
        List<string> list = new List<string>();
        string[] files = Directory.GetFiles(Application.persistentDataPath + "/Floors", "*.ncs");
        for (int i = 0; i < files.Length; i++)
        {
            string text = files[i];
            string text2;
            if (Crypt.Check(text))
            {
                text2 = "✔ ";
            }
            else
            {
                text2 = "";
            }
            text2 += text.Split(new char[]
            {
                    '\\'
            })[1].Split(new char[]
            {
                    '.'
            })[0];
            list.Add(text2);
        }
        this.dlist.GetComponent<Dropdown>().ClearOptions();
        this.dlist.GetComponent<Dropdown>().AddOptions(list);
        try
        {
        }
        catch
        {
            Error.ErrorMassage("Ошибка чтения файлов. Проверьте наличие и целостность содержимого папки" + Application.persistentDataPath + "/Floors");
        }
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

    public void RemoveGame()
    {
        try
        {
            string text = this.dlist.GetComponent<Dropdown>().captionText.text;
            if (text.IndexOf("✔") >= 0)
            {
                text = text.Remove(0, 2);
            }
            File.Delete(Application.persistentDataPath + "/Floors/" + text + ".ncs");
            this.Init();
        }
        catch
        {
            Debug.Log("Ошибка удаления");
        }
    }
}
