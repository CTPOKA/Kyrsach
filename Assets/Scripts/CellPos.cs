using System;
using UnityEngine;
using System.Collections;

public class CellPos : MonoBehaviour
{
    public int X;

    public int Y;

    public int dx;

    public int dy;

    public float Z;

    public void SetPos(int x, int y, float z = -1f)
    {
        this.X = x;
        this.Y = y;
        if (z >= 0f)
        {
            this.Z = z;
        }
        this.UpdatePos();
    }

    private void UpdatePos()
    {
        base.transform.position = new Vector3((float)this.X * 1.15f, (float)this.Y * 0.78f, (float)this.Y - this.Z);
    }

    public void Move(int mx, int my)
    {
        dx = mx;
        dy = my;
        StartCoroutine(MoveAnim());
        IEnumerator MoveAnim()
        {
            while (Player.timeout > 0f)
            {
                float x = (X * 1.15f) + ((dx * 1.15f) * (1f - Player.timeout));
                float y = (Y * 0.78f) + ((dy * 0.78f) * (1f - Player.timeout));
                float z = 0f;
                if (this.dy < 0)
                {
                    z = 1f;
                }
                transform.position = new Vector3(x, y, (Y - Z) - z);
                yield return new WaitForSeconds(0.05f);
            }
            SetPos(X + dx, Y + dy);
        }
    }
}
