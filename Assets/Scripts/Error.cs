using UnityEngine;
using UnityEngine.UI;

public class Error : MonoBehaviour
{
    private static GameObject err;

    private void Awake()
    {
        Error.err = base.gameObject;
    }

    public static void ErrorMassage(string massage)
    {
        Error.err.GetComponentInParent<Canvas>().enabled = true;
        Error.err.GetComponentInChildren<Text>().text = massage;
        Debug.Log(massage);
    }
}

