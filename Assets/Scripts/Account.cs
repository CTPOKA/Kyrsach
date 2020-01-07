using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Account : MonoBehaviour
{
    public static TestClient.Client client = new TestClient.Client("http://localhost/TestServer/");
    public static string user = "0";
    public InputField Name;
    public InputField Password;
    public Text status;
    public GameObject Form;
    public GameObject button1;
    public GameObject button2;
    private bool correct;
    const string noauth = "Не авторизован";

    private void Awake()
    {
        Status();
    }
    public void Login ()
    {
        try
        {
            if (client.Login(Name.text, Password.text) == 1) Error.ErrorMassage("Не правильное имя пользователя или пароль");
            else Form.SetActive(false);
        }
        catch
        {
            Error.ErrorMassage("Потеряно соединение с сервером");
        }
        Status();
    }
    public void Register()
    {
        if (correct)
            try
            {
                if (client.Register(Name.text, Password.text) == 1) Error.ErrorMassage("Ошибка создания пользователя");
                else Form.SetActive(false);
            }
            catch
            {
                Error.ErrorMassage("Потеряно соединение с сервером");
            }
        else Error.ErrorMassage("Имя и пароль не соответствуют требованиям");
        Status();
    }
    public void Check()
    {
        bool cl = false;
        bool cp;
        string name = Name.text;
        char[] chr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();
        foreach (var c in chr)
        {
            if (name != "" && name[0] == c) cl = true;
        }
        chr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 - _".ToCharArray();
        if (name.Length < 4 || name.Trim(chr) != "") cl = false;
        cp = Password.text.Length >= 6;
        Password.GetComponent<InputField>().textComponent.color = cp ? Color.black : Color.red;
        Name.GetComponent<InputField>().textComponent.color = cl ? Color.black : Color.red;
        correct = cp && cl;
    }
    public void Logout()
    {
        client.Logout();
        Status();
    }
    void Status()
    {
        try
        {
            if ((user = client.token.Split('+')[0]) == "0")
            {
                status.text = noauth;
                status.color = Color.red;
                button1.SetActive(true);
                button2.SetActive(false);
                return;
            }
        }
        catch
        {
            Error.ErrorMassage("Потеряно соединение с сервером");
            return;
        }
        status.text = user;
        status.color = Color.black;
        button2.SetActive(true);
        button1.SetActive(false);
    }
}
