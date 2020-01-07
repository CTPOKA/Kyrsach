using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Level : MonoBehaviour
{
    public Text Name;
    public Text Autor;
    public Button button1;
    public Button button2;
    public Button button3;
    public GameObject Form;
    public InputField ipassword;

    public void Init (string name, string autor)
    {
        Name.text = name;
        Autor.text = autor;
    }
    private void Start()
    {
        Check();
        if (Account.user != Autor.text) button2.GetComponent<Button>().interactable = false;
        else button2.interactable = true;
    }
    public void Load()
    {
        string password = Form.activeSelf ? ipassword.text : "";
        string name = Name.text;
        try
        {
            string level = Account.client.GetLevel(name, password);
            if (level != "" && level != null)
            {
                Crypt.Write("Floors/" + name.Trim('~') + ".ncs", level);
                Form.SetActive(false);
            }
            else if (Form.activeSelf) Error.ErrorMassage("Ошибка доступа");
            else
            {
                Form.SetActive(true);
                Level lvl = this;
                button3.onClick.RemoveAllListeners();
                button3.onClick.AddListener(() => lvl.Load());
            }
        }
        catch
        {
            Error.ErrorMassage("Потеряно соединение с сервером");
        }
        if (button1 != null)
        Check();
    }
    public void Delete()
    {
        try
        {
            Account.client.DeleteLevel(Name.text);
        }
        catch
        {
            Error.ErrorMassage("Потеряно соединение с сервером");
        }
    }
    public void Check()
    {
        string[] files = Directory.GetFiles(Application.persistentDataPath + "/Floors", "*.ncs");
        foreach(var f in files)
        {
            if (f.Split('\\')[1].Split('.')[0] == Name.text.Trim('~')) button1.interactable = false;
        }
    }
}
