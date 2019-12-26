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

    private void Update()
    {
        switch (this.rotation)
        {
            case 0:
                base.GetComponentInChildren<SpriteRenderer>().sprite = this.sUp;
                return;
            case 1:
                base.GetComponentInChildren<SpriteRenderer>().sprite = this.sDown;
                return;
            case 2:
                base.GetComponentInChildren<SpriteRenderer>().sprite = this.sRight;
                return;
            case 3:
                base.GetComponentInChildren<SpriteRenderer>().sprite = this.sLeft;
                return;
            default:
                return;
        }
    }

    public static int Flip(int rot)
    {
        return rot - 2 * (rot % 2) + 1;
    }
}

