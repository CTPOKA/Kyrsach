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
        StreamReader reader1 = new StreamReader(stream);
        bool ch = false;
        try
        {
            string str = reader1.ReadToEnd();
            ch = str.Split('|')[0].Split(' ')[1] == "+";
            stream.Close();
            reader1.Close();
            stream1.Close();
        }
        catch
        {
            Error.ErrorMassage("Ошибка чтения файла " + filepath);
        }
        return ch;
    }

    public static string Read(string filepath)
    {
        FileStream stream1 = new FileStream(Application.persistentDataPath + "/" + filepath, (FileMode)FileMode.Open, (FileAccess)FileAccess.Read);
        CryptoStream stream = new CryptoStream((Stream)stream1, crypt.CreateDecryptor(), (CryptoStreamMode)CryptoStreamMode.Read);
        StreamReader reader1 = new StreamReader((Stream)stream);
        string str = reader1.ReadToEnd();
        stream.Close();
        reader1.Close();
        stream1.Close();
        return str;
    }

    public static void Write(string filepath, string data)
    {
        FileStream stream1 = new FileStream(Application.persistentDataPath + "/" + filepath, (FileMode)FileMode.OpenOrCreate, (FileAccess)FileAccess.Write);
        CryptoStream stream2 = new CryptoStream((Stream)stream1, crypt.CreateEncryptor(), (CryptoStreamMode)CryptoStreamMode.Write);
        StreamWriter writer1 = new StreamWriter((Stream)stream2);
        writer1.Write(data);
        writer1.Close();
        stream2.Close();
        stream1.Close();
    }
}
