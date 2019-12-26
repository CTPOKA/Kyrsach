using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obj5 : MonoBehaviour
{
    public int type;
    bool locked = false;
    int x, y;
    GameObject obj;
    public void Start()
    {
        x = GetComponent<CellPos>().X;
        y = GetComponent<CellPos>().Y;
        if (RoomSpace.M.GetCell(x, y + 1)==3)
        {
            GetComponent<Rotation>().rotation = Rotation.Right;
        }
        if (RoomSpace.M.GetCell(x + 1, y)==3)
        {
            GetComponent<Rotation>().rotation = Rotation.Up;
        }
        if (RoomSpace.M.GetCell(x, y - 1)==3)
        {
            GetComponent<Rotation>().rotation = Rotation.Right;
        }
        if (RoomSpace.M.GetCell(x - 1, y)==3)
        {
            GetComponent<Rotation>().rotation = Rotation.Up;
        }
        GetComponent<CellPos>().Z = 0.1f;
        GetComponent<CellPos>().SetPos(x, y);
        obj = Instantiate(RoomSpace.M.objects[0]) as GameObject;
        obj.GetComponent<CellPos>().SetPos(x, y);
    }
    void Update()
    {
        if (type == 5)
        if (RoomSpace.M.tasks == 0) locked = false;
        else locked = true;
        if (type == 13)
        if (RoomSpace.M.keys[0] - RoomSpace.M.keys[1] == 0) locked = false;
        else locked = true;
        if (RoomSpace.M.GetCell(x, y) == 2)
        {
            if (GetComponent<Rotation>().rotation <= 1)
            {
                GetComponent<CellPos>().SetPos(x - 1, y);
            }
            else GetComponent<CellPos>().SetPos(x, y - 1);
        }
        else if (!locked)
        {
                if (RoomSpace.M.GetCell(x, y) != 2)
                {
                    RoomSpace.M.SetCell(x, y, 0);
                }
                if (GetComponent<Rotation>().rotation > 1)
                {
                    if (RoomSpace.M.GetCell(x + 1, y) == 2 || RoomSpace.M.GetCell(x - 1, y) == 2)
                    {
                    if (Player.timeout == 1) GetComponent<CellPos>().Move(0, Mathf.Abs(y - GetComponent<CellPos>().Y) - 1);
                    }
                    else
                    {
                    if (Player.timeout == 1) GetComponent<CellPos>().Move(0, y - GetComponent<CellPos>().Y);
                        RoomSpace.M.SetCell(x, y, -1);
                    }
                }
                else
                {
                    if (RoomSpace.M.GetCell(x, y + 1) == 2 || RoomSpace.M.GetCell(x, y - 1) == 2)
                    {
                    if (Player.timeout == 1) GetComponent<CellPos>().Move(Mathf.Abs(x - GetComponent<CellPos>().X) - 1, 0);
                    }
                    else
                    {
                    if (Player.timeout == 1) GetComponent<CellPos>().Move(x - GetComponent<CellPos>().X, 0);
                        RoomSpace.M.SetCell(x, y, -1);
                    }
                }
            GetComponentInChildren<BigObject>().enabled = false;
            if (type == 5) GetComponentInChildren<SpriteRenderer>().color = Color.green;
        } else
        {   if (RoomSpace.M.GetCell(x, y) != 2)
            {
                if (Player.timeout == 1)
                {
                    GetComponent<CellPos>().Move(x - GetComponent<CellPos>().X, y - GetComponent<CellPos>().Y);
                }
                RoomSpace.M.SetCell(x, y, -1);
            }
            if (type == 5) GetComponentInChildren<SpriteRenderer>().color = Color.red;
            GetComponentInChildren<BigObject>().enabled = true;
        }
    }
    private void OnDestroy()
    {
        Destroy(obj);
    }
}
