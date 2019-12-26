using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class Crypt : MonoBehaviour
{
    public static DESCryptoServiceProvider crypt = new DESCryptoServiceProvider();

    private void Awake()
    {
        Crypt.crypt.Key = Encoding.ASCII.GetBytes("ABCDEFGH");
        Crypt.crypt.IV = Encoding.ASCII.GetBytes("ABCDEFGH");
    }

    public static void Write(string filepath, string data)
    {
        FileStream expr_17 = new FileStream(Application.persistentDataPath + "/" + filepath, FileMode.OpenOrCreate, FileAccess.Write);
        CryptoStream expr_28 = new CryptoStream(expr_17, Crypt.crypt.CreateEncryptor(), CryptoStreamMode.Write);
        StreamWriter expr_2E = new StreamWriter(expr_28);
        expr_2E.Write(data);
        expr_2E.Close();
        expr_28.Close();
        expr_17.Close();
    }

    public static string Read(string filepath)
    {
        FileStream expr_17 = new FileStream(Application.persistentDataPath + "/" + filepath, FileMode.Open, FileAccess.Read);
        CryptoStream cryptoStream = new CryptoStream(expr_17, Crypt.crypt.CreateDecryptor(), CryptoStreamMode.Read);
        StreamReader expr_2F = new StreamReader(cryptoStream);
        string result = expr_2F.ReadToEnd();
        cryptoStream.Close();
        expr_2F.Close();
        expr_17.Close();
        return result;
    }

    public static bool Check(string filepath)
    {
        return new StreamReader(new CryptoStream(new FileStream(filepath, FileMode.Open, FileAccess.Read), Crypt.crypt.CreateDecryptor(), CryptoStreamMode.Read)).ReadToEnd().Split(new char[]
        {
            '|'
        })[0].Split(new char[]
        {
            ' '
        })[1] == "+";
    }
}
