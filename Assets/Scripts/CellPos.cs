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
        StartCoroutine(MoveAnim());
        IEnumerator MoveAnim()
        {
            yield return new WaitForSeconds(0.1f);
        }
    }
}
