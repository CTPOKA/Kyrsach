using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obj6 : MonoBehaviour
{
    int x, y;
    bool on = true;
    private void Start()
    {
        GetComponent<SpriteRenderer>().color = Color.green;
        x = GetComponent<CellPos>().X;
        y = GetComponent<CellPos>().Y;
    }
    void Update()
    {
        if (!RoomSpace.intangible[RoomSpace.M.GetCell(x,y)] && !on)
        {
            GetComponent<SpriteRenderer>().color = Color.green;
            RoomSpace.M.tasks--;
            on = true;
        } 
        else if (RoomSpace.intangible[RoomSpace.M.GetCell(x, y)] && on)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            RoomSpace.M.tasks++;
            on = false;
        }
    }
}
