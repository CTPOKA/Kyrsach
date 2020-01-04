using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class Crypt : MonoBehaviour
{
    public static DESCryptoServiceProvider crypt = new DESCryptoServiceProvider();

    private void Awake()
    {
        crypt.Key = Encoding.ASCII.GetBytes("ABCDEFGH");
        crypt.IV = Encoding.ASCII.GetBytes("ABCDEFGH");
    }

    public static bool Check(string filepath)
    {
        FileStream stream1 = new FileStream(filepath, FileMode.Open, FileAccess.Read);
        CryptoStream stream = new CryptoStream(stream1, crypt.CreateDecryptor(), CryptoStreamMode.Read);
        StreamReader sr = new StreamReader(stream);
        bool ch = false;
        try
        {
            string str = sr.ReadToEnd();
            stream1.Close();
            stream.Close();
            sr.Close();
            ch = str.Split('|')[0].Split(' ')[1] == "+";
        }
        catch
        {
            stream1.Close();
            stream.Close();
            sr.Close();
            Error.ErrorMassage("Ошибка чтения файла " + filepath);
        }
        return ch;
    }

    public static string Read(string filepath)
    {
        FileStream stream1 = new FileStream(Application.persistentDataPath + "/" + filepath, FileMode.Open, FileAccess.Read);
        CryptoStream stream = new CryptoStream(stream1, crypt.CreateDecryptor(), CryptoStreamMode.Read);
        StreamReader sr = new StreamReader(stream);
        try
        {
            string str = sr.ReadToEnd();
            stream1.Close();
            stream.Close();
            sr.Close();
            return str;
        } 
        catch
        {
            stream1.Close();
            stream.Close();
            sr.Close();
            Error.ErrorMassage("Ошибка чтения файла " + filepath);
            return null;
        }
    }

    public static void Write(string filepath, string data)
    {
        FileStream stream1 = new FileStream(Application.persistentDataPath + "/" + filepath, (FileMode)FileMode.OpenOrCreate, (FileAccess)FileAccess.Write);
        CryptoStream stream2 = new CryptoStream((Stream)stream1, crypt.CreateEncryptor(), (CryptoStreamMode)CryptoStreamMode.Write);
        StreamWriter writer1 = new StreamWriter((Stream)stream2);
        try
        {
            writer1.Write(data);
            writer1.Close();
            stream2.Close();
            stream1.Close();
        }
        catch 
        {
            writer1.Close();
            stream2.Close();
            stream1.Close();
            Error.ErrorMassage("Ошибка сохранения файла " + filepath);
        }

    }
}
