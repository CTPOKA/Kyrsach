using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LevelsList : MonoBehaviour
{
    public GameObject level;
    List<GameObject> list = new List<GameObject>();
    public InputField find;
    public InputField share;
    public InputField password;
    public Dropdown select;
    public GameObject Form1;
    public GameObject Form2;
    public GameObject Form3;
    bool conetion = false;

    const float size = 36;
    private void Awake()
    {
        StartCoroutine(Refresh());
    }

    public void Init()
    {
        string[] llslist;
        foreach (var l in list)
        {
            Destroy(l);
        }
        list.Clear();
        string str = Account.client.List();
        if (str == null)
        {
            conetion = false;
            Error.ErrorMassage("Потеряно соединение с сервером");
            return;
        }
        else if (str == "0") return;
        conetion = true;
        llslist = str.Split('|');
        int n = llslist.Length;
        int x = 0;
        for (int i = 1; i < n; i++)
            if (find.text == "" || llslist[i].Split('&')[0].IndexOf(find.text) != -1) x++;
        transform.localScale = new Vector2(1, x);
        int y = 0;
        for (int i = 1; i < n; i++)
        {
            string nam = llslist[i].Split('&')[0];
            if (find.text == ""|| nam.IndexOf(find.text)!=-1)
            {
                string aut = llslist[i].Split('&')[1];
                GameObject obj = Instantiate(level);
                obj.transform.SetParent(transform);
                obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -y * size / x);
                obj.GetComponent<Level>().Init(nam, aut);
                list.Add(obj);
                y++;
            }
        }
    }
    public void Share()
    {
        string nam = share.text;
        string file = select.captionText.text.Remove(0, 2);
        char[] chr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 - _".ToCharArray();
        if (nam.Length < 4 || nam.Trim(chr) != "")
        {
            Error.ErrorMassage("Название не соответствует требованиям");
            return;
        }
        if (password.text != "") nam = "~" + nam;
        string level;
        try
        {
            level = Crypt.Read("Floors/" + file + ".ncs");
            level = level.Replace('+', '-').Replace(' ', '+');
            int res = Account.client.Post(nam, level, password.text);
            if (res == -1)
            {
                Error.ErrorMassage("Уровень с таким названием уже имеется");
            }
            else if (res == -2)
            {
                Error.ErrorMassage("Такой уровень уже имеется");
            }
        }
        catch
        {
            Error.ErrorMassage("Потеряно соединение с сервером");
        }
    }
    public void Check()
    {
        string lvl = select.captionText.text;
        if (Account.user == "0")
        {
            Error.ErrorMassage("Необходимо авторизоватся");
            Form1.SetActive(true);
        }
        else if (lvl == "")
        {
            Error.ErrorMassage("Уровень не выбран");
        }
        else if (lvl.IndexOf("✔") < 0)
        {
            Error.ErrorMassage("Публикуемый уровень должен быть пройден");
        }
        else
        {
            share.text = lvl.Remove(0, 2);
            Form2.SetActive(true);
        }
    }
    IEnumerator Refresh()
    {
        while (true)
        {
            if (gameObject.activeSelf && !Form3.activeSelf && conetion)
            Init();
            yield return new WaitForSeconds(1f);
        }
    }
}
