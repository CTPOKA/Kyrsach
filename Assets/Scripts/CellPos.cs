using System.Collections;
using UnityEngine;

public class CellPos : MonoBehaviour
{
    public int X, Y, dx, dy;
    public float Z;
    public void SetPos(int x, int y, float z = -1)
    {
        this.X = x;
        this.Y = y;
        if (z >= 0)
        {
            this.Z = z;
        }
        UpdatePos();
    }

    void UpdatePos()
    {
        transform.position = new Vector3(X * RoomSpace.cellsizeX, Y * RoomSpace.cellsizeY, Y - Z);
    }
    public void Move (int mx, int my)
    {
        StartCoroutine(MoveAnim(mx, my));
        IEnumerator MoveAnim(int dx, int dy)
        {
            this.dx = mx; this.dy = my;
            while (Player.timeout > 0)
            {
                float x = X * RoomSpace.cellsizeX + dx * RoomSpace.cellsizeX * (1 - Player.timeout);
                float y = Y * RoomSpace.cellsizeY + dy * RoomSpace.cellsizeY * (1 - Player.timeout);
                float yz = 0;
                if (dy < 0)
                {
                    yz = 1;
                }
                transform.position = new Vector3(x, y, Y - Z - yz);
                yield return new WaitForSeconds(0.05f);
            }
            SetPos(X + dx, Y + dy);
        }
    }
}
