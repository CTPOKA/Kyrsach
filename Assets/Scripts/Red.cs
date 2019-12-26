using System;
using UnityEngine;
using UnityEngine.UI;

public class Red : MonoBehaviour
{
    public Text keys;

    public Image pain;

    private static Color red = new Color(1f, 0f, 0f, 0f);

    private void Update()
    {
        this.pain.GetComponent<Image>().color = Red.red;
        this.keys.GetComponent<Text>().text = (RoomSpace.M.keys[0] - RoomSpace.M.keys[1]).ToString();
        Red.red.a = Red.red.a - 0.01f;
    }

    public static void Pain()
    {
        Red.red = new Color(1f, 0f, 0f, 0.4f);
    }
}
