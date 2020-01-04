using UnityEngine;
using UnityEngine.UI;

public class Error : MonoBehaviour
{
    private static GameObject err;

    private void Awake()
    {
        err = gameObject;
    }

    public static void ErrorMassage(string massage)
    {
        if (err != null)
        {
            err.GetComponentInParent<Canvas>().enabled = true;
            err.GetComponentInChildren<Text>().text = massage;
        }
        Debug.Log(massage);
    }
}

