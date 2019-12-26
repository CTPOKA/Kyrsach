using System;
using UnityEngine;

public class Obj7 : MonoBehaviour
{
    private int x;

    private int y;

    public Sprite s;

    private void Start()
    {
        this.x = base.GetComponent<CellPos>().X;
        this.y = base.GetComponent<CellPos>().Y;
    }

    private void Update()
    {
        if (Player.timeout <= 0f && RoomSpace.M.GetCell(this.x, this.y, 1) == 4)
        {
            RoomSpace.M.SetObject(this.x, this.y, 0, 1);
            RoomSpace.M.SetObject(this.x, this.y, 1, 0).GetComponent<SpriteRenderer>().sprite = this.s;
        }
    }
}
