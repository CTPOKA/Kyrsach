using System;
using UnityEngine;

public class Obj6 : MonoBehaviour
{
    private int x;

    private int y;

    private bool on = true;

    private void Start()
    {
        base.GetComponent<SpriteRenderer>().color = Color.green;
        this.x = base.GetComponent<CellPos>().X;
        this.y = base.GetComponent<CellPos>().Y;
    }

    private void Update()
    {
        if (!RoomSpace.intangible[RoomSpace.M.GetCell(this.x, this.y, 1)] && !this.on)
        {
            base.GetComponent<SpriteRenderer>().color = Color.green;
            RoomSpace.M.tasks--;
            this.on = true;
            return;
        }
        if (RoomSpace.intangible[RoomSpace.M.GetCell(this.x, this.y, 1)] && this.on)
        {
            base.GetComponent<SpriteRenderer>().color = Color.red;
            RoomSpace.M.tasks++;
            this.on = false;
        }
    }
}
