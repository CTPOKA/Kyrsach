using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using System.IO;

public class Crypt : MonoBehaviour
{
    static public DESCryptoServiceProvider crypt = new DESCryptoServiceProvider();
    private void Awake()
    {
        crypt.Key = Encoding.ASCII.GetBytes("ABCDEFGH");
        crypt.IV = Encoding.ASCII.GetBytes("ABCDEFGH");
    }
    static public void Write(string filepath, string data)
    {
        FileStream stream = new FileStream(Application.persistentDataPath+"/"+ filepath, FileMode.OpenOrCreate, FileAccess.Write);
        CryptoStream crStream = new CryptoStream(stream, crypt.CreateEncryptor(), CryptoStreamMode.Write);
        StreamWriter sw = new StreamWriter(crStream);

        sw.Write(data);

        sw.Close();
        crStream.Close();
        stream.Close();
    }
    public static string Read(string filepath)
    {
        FileStream stream = new FileStream(Application.persistentDataPath + "/" + filepath, FileMode.Open, FileAccess.Read);
        CryptoStream crStream = new CryptoStream(stream, crypt.CreateDecryptor(), CryptoStreamMode.Read);
        StreamReader sr = new StreamReader(crStream);

        string data = sr.ReadToEnd();

        crStream.Close();
        sr.Close();
        stream.Close();

        return data;
    }
    static public bool Check(string filepath)
    {
        FileStream stream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
        CryptoStream crStream = new CryptoStream(stream, crypt.CreateDecryptor(), CryptoStreamMode.Read);
        StreamReader sr = new StreamReader(crStream);

        bool res = sr.ReadToEnd().Split('|')[0].Split(' ')[1] == "+";
        crStream.Close();
        sr.Close();
        stream.Close();
        try
        {
            if (res) return true;
        } catch
        {
            return false;
        }
        return false;
    }
}
