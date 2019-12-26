using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigObject : MonoBehaviour
{
    int x, y;
    Color color;
    private void Start()
    {
        x = GetComponent<CellPos>().X;
        y = GetComponent<CellPos>().Y;
    }
    void Update()
    {
        color = GetComponentInChildren<SpriteRenderer>().color;
        if ((RoomSpace.M.GetCell(x, y + 1) == 0 || RoomSpace.M.GetCell(x, y + 1) == -1 || RoomSpace.bigobject[RoomSpace.M.GetCell(x, y + 1)]) && (RoomSpace.M.GetCell(x, y + 1, 0) == 0 || RoomSpace.M.GetCell(x, y + 1, 0) == 1 || RoomSpace.M.GetCell(x, y + 1, 0) == -1 || RoomSpace.bigobject[RoomSpace.M.GetCell(x, y + 1, 0)]))
        {
            color.a = 1;
        }
        else color.a = 0.25f;
        GetComponentInChildren<SpriteRenderer>().color = color;
    }
}
