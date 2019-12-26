using System;
using UnityEngine;

public class BigObject : MonoBehaviour
{
    private int x;

    private int y;

    private Color color;

    private void Start()
    {
        this.x = base.GetComponent<CellPos>().X;
        this.y = base.GetComponent<CellPos>().Y;
    }

    private void Update()
    {
        this.color = base.GetComponentInChildren<SpriteRenderer>().color;
        if ((RoomSpace.M.GetCell(this.x, this.y + 1, 1) == 0 || RoomSpace.M.GetCell(this.x, this.y + 1, 1) == -1 || RoomSpace.bigobject[RoomSpace.M.GetCell(this.x, this.y + 1, 1)]) && (RoomSpace.M.GetCell(this.x, this.y + 1, 0) == 0 || RoomSpace.M.GetCell(this.x, this.y + 1, 0) == 1 || RoomSpace.M.GetCell(this.x, this.y + 1, 0) == -1 || RoomSpace.bigobject[RoomSpace.M.GetCell(this.x, this.y + 1, 0)]))
        {
            this.color.a = 1f;
        }
        else
        {
            this.color.a = 0.25f;
        }
        base.GetComponentInChildren<SpriteRenderer>().color = this.color;
    }
}
