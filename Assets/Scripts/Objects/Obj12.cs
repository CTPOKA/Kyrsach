using System;
using UnityEngine;

public class Obj12 : MonoBehaviour
{
    private int x;

    private int y;

    private void Start()
    {
        this.x = base.GetComponent<CellPos>().X;
        this.y = base.GetComponent<CellPos>().Y;
    }

    private void Update()
    {
        if (Player.timeout < 0f && RoomSpace.M.GetCell(this.x, this.y, 1) != 12)
        {
            Sounds.A.Play(4);
            RoomSpace.M.keys[1]++;
            UnityEngine.Object.Destroy(base.gameObject);
        }
    }
}
