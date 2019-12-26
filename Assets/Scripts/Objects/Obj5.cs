using System;
using UnityEngine;

public class Obj5 : MonoBehaviour
{
    public int type;

    private bool locked;

    private int x;

    private int y;

    private GameObject obj;

    public void Start()
    {
        this.x = base.GetComponent<CellPos>().X;
        this.y = base.GetComponent<CellPos>().Y;
        if (RoomSpace.M.GetCell(this.x, this.y + 1, 1) == 3)
        {
            base.GetComponent<Rotation>().rotation = 2;
        }
        if (RoomSpace.M.GetCell(this.x + 1, this.y, 1) == 3)
        {
            base.GetComponent<Rotation>().rotation = 0;
        }
        if (RoomSpace.M.GetCell(this.x, this.y - 1, 1) == 3)
        {
            base.GetComponent<Rotation>().rotation = 2;
        }
        if (RoomSpace.M.GetCell(this.x - 1, this.y, 1) == 3)
        {
            base.GetComponent<Rotation>().rotation = 0;
        }
        base.GetComponent<CellPos>().Z = 0.1f;
        base.GetComponent<CellPos>().SetPos(this.x, this.y, -1f);
        this.obj = UnityEngine.Object.Instantiate<GameObject>(RoomSpace.M.objects[0]);
        this.obj.GetComponent<CellPos>().SetPos(this.x, this.y, -1f);
    }

    private void Update()
    {
        if (this.type == 5)
        {
            if (RoomSpace.M.tasks == 0)
            {
                this.locked = false;
            }
            else
            {
                this.locked = true;
            }
        }
        if (this.type == 13)
        {
            if (RoomSpace.M.keys[0] - RoomSpace.M.keys[1] == 0)
            {
                this.locked = false;
            }
            else
            {
                this.locked = true;
            }
        }
        if (RoomSpace.M.GetCell(this.x, this.y, 1) != 2)
        {
            if (!this.locked)
            {
                if (RoomSpace.M.GetCell(this.x, this.y, 1) != 2)
                {
                    RoomSpace.M.SetCell(this.x, this.y, 0, 1);
                }
                if (base.GetComponent<Rotation>().rotation > 1)
                {
                    if (RoomSpace.M.GetCell(this.x + 1, this.y, 1) == 2 || RoomSpace.M.GetCell(this.x - 1, this.y, 1) == 2)
                    {
                        if (Player.timeout == 1f)
                        {
                            base.GetComponent<CellPos>().Move(0, Mathf.Abs(this.y - base.GetComponent<CellPos>().Y) - 1);
                        }
                    }
                    else
                    {
                        if (Player.timeout == 1f)
                        {
                            base.GetComponent<CellPos>().Move(0, this.y - base.GetComponent<CellPos>().Y);
                        }
                        RoomSpace.M.SetCell(this.x, this.y, -1, 1);
                    }
                }
                else if (RoomSpace.M.GetCell(this.x, this.y + 1, 1) == 2 || RoomSpace.M.GetCell(this.x, this.y - 1, 1) == 2)
                {
                    if (Player.timeout == 1f)
                    {
                        base.GetComponent<CellPos>().Move(Mathf.Abs(this.x - base.GetComponent<CellPos>().X) - 1, 0);
                    }
                }
                else
                {
                    if (Player.timeout == 1f)
                    {
                        base.GetComponent<CellPos>().Move(this.x - base.GetComponent<CellPos>().X, 0);
                    }
                    RoomSpace.M.SetCell(this.x, this.y, -1, 1);
                }
                base.GetComponentInChildren<BigObject>().enabled = false;
                if (this.type == 5)
                {
                    base.GetComponentInChildren<SpriteRenderer>().color = Color.green;
                    return;
                }
            }
            else
            {
                if (RoomSpace.M.GetCell(this.x, this.y, 1) != 2)
                {
                    if (Player.timeout == 1f)
                    {
                        base.GetComponent<CellPos>().Move(this.x - base.GetComponent<CellPos>().X, this.y - base.GetComponent<CellPos>().Y);
                    }
                    RoomSpace.M.SetCell(this.x, this.y, -1, 1);
                }
                if (this.type == 5)
                {
                    base.GetComponentInChildren<SpriteRenderer>().color = Color.red;
                }
                base.GetComponentInChildren<BigObject>().enabled = true;
            }
            return;
        }
        if (base.GetComponent<Rotation>().rotation <= 1)
        {
            base.GetComponent<CellPos>().SetPos(this.x - 1, this.y, -1f);
            return;
        }
        base.GetComponent<CellPos>().SetPos(this.x, this.y - 1, -1f);
    }

    private void OnDestroy()
    {
        UnityEngine.Object.Destroy(this.obj);
    }
}