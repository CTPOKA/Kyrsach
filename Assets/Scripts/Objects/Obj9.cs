using System;
using UnityEngine;

public class Obj9 : MonoBehaviour
{
    private int x;

    private int y;

    private bool broken;

    private void Start()
    {
        this.x = base.GetComponent<CellPos>().X;
        this.y = base.GetComponent<CellPos>().Y;
    }

    private void Update()
    {
        if (Player.timeout < -0.1f)
        {
            if (!RoomSpace.intangible[RoomSpace.M.GetCell(this.x, this.y, 1)])
            {
                this.broken = true;
            }
            if (RoomSpace.intangible[RoomSpace.M.GetCell(this.x, this.y, 1)] && this.broken)
            {
                Sounds.A.Play(3);
                RoomSpace.M.SetObject(this.x, this.y, 7, 0);
            }
        }
    }
}
