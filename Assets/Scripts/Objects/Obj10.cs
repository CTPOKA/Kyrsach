using System;
using UnityEngine;

public class Obj10 : MonoBehaviour
{
    private int x;

    private int y;

    private GameObject obj;

    private void Start()
    {
        this.x = base.GetComponent<CellPos>().X;
        this.y = base.GetComponent<CellPos>().Y;
    }

    private void Update()
    {
        if (!RoomSpace.intangible[RoomSpace.M.GetCell(this.x, this.y, 1)] && (this.obj = RoomSpace.M.GetObject(this.x, this.y, 1)) != null)
        {
            RoomSpace.M.Move(this.x, this.y, this.obj.GetComponent<CellPos>().dx, this.obj.GetComponent<CellPos>().dy, 1);
        }
    }
}
