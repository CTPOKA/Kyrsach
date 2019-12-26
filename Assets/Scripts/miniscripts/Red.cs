using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Red : MonoBehaviour
{
    public Text keys;
    public Image pain;
    static Color red = new Color(1, 0, 0, 0);
    void Update()
    {
        pain.GetComponent<Image>().color = red;
        keys.GetComponent<Text>().text = (RoomSpace.M.keys[0] - RoomSpace.M.keys[1]).ToString();
        red.a -= 0.01f;
    }

    static public void Pain()
    {
        red = new Color(1, 0, 0, 0.4f);
    }
}
