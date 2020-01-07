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
        bool ch = false;
        try
        {
            string str;
            using (FileStream stream1 = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            using (CryptoStream stream = new CryptoStream(stream1, crypt.CreateDecryptor(), CryptoStreamMode.Read))
            //string stream = filepath;
            using (StreamReader sr = new StreamReader(stream))
                str = sr.ReadToEnd();
            ch = str.Split('|')[0].Split(' ')[1] == "+";
        }
        catch
        {
            Error.ErrorMassage("Ошибка чтения файла " + filepath);
        }
        return ch;
    }

    public static string Read(string filepath)
    {
        string str = null;
        try
        {
            using (FileStream stream1 = new FileStream(Application.persistentDataPath + "/" + filepath, FileMode.Open, FileAccess.Read))
            using (CryptoStream stream = new CryptoStream(stream1, crypt.CreateDecryptor(), CryptoStreamMode.Read))
            //string stream = Application.persistentDataPath + "/" + filepath;
            using (StreamReader sr = new StreamReader(stream))
                str = sr.ReadToEnd();
        }
        catch
        {
            Error.ErrorMassage("Ошибка чтения файла " + filepath);
        }
        return str;
    }

    public static void Write(string filepath, string data)
    {
        try
        {
            using (FileStream stream1 = new FileStream(Application.persistentDataPath + "/" + filepath, FileMode.OpenOrCreate, FileAccess.Write))
            using (CryptoStream stream = new CryptoStream(stream1, crypt.CreateEncryptor(), CryptoStreamMode.Write))
            //string stream = Application.persistentDataPath + "/" + filepath;
            using (StreamWriter sw = new StreamWriter(stream))
                sw.Write(data);
        }
        catch 
        {
            Error.ErrorMassage("Ошибка сохранения файла " + filepath);
        }

    }
}
