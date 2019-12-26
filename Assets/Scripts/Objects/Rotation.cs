using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public const int Up = 0;
    public const int Down = 1;
    public const int Left = 3;
    public const int Right = 2;
    public Sprite sUp;
    public Sprite sDown;
    public Sprite sLeft;
    public Sprite sRight;
    public int rotation;
    void Update()
    {
        switch (rotation)
        {
            case (Up):
                GetComponentInChildren<SpriteRenderer>().sprite = sUp;
                break;
            case (Right):
                GetComponentInChildren<SpriteRenderer>().sprite = sRight;
                break;
            case (Down):
                GetComponentInChildren<SpriteRenderer>().sprite = sDown;
                break;
            case (Left):
                GetComponentInChildren<SpriteRenderer>().sprite = sLeft;
                break;
        }
    }
    public static int Flip(int rot)
    {
        return (rot - 2 * (rot % 2) + 1);
    }
}
