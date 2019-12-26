using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int x;

    private int y;

    private int dx;

    private int dy;

    public static float timeout;

    public static int speed = 5;

    private void Update()
    {
        this.x = base.GetComponent<CellPos>().X;
        this.y = base.GetComponent<CellPos>().Y;
        try
        {
            if (RoomSpace.killing[RoomSpace.M.GetCell(this.x, this.y, 0)])
            {
                this.Death();
            }
        }
        catch
        {
            UnityEngine.Object.Destroy(this);
            Error.ErrorMassage("Ошибка расстановки. Уровень не правильно построен");
        }
        this.dx = 0;
        this.dy = 0;
        if (Player.timeout < -0.5f)
        {
            if (Input.GetKey("down"))
            {
                this.dy = -1;
                base.GetComponent<Rotation>().rotation = 1;
            }
            else if (Input.GetKey("up"))
            {
                this.dy = 1;
                base.GetComponent<Rotation>().rotation = 0;
            }
            else if (Input.GetKey("left"))
            {
                this.dx = -1;
                base.GetComponent<Rotation>().rotation = 3;
            }
            else if (Input.GetKey("right"))
            {
                this.dx = 1;
                base.GetComponent<Rotation>().rotation = 2;
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                this.Death();
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log(RoomSpace.bigobject[2]);
            }
            if (this.dx != 0 || this.dy != 0)
            {
                Player.timeout = 1f;
                this.Move();
                return;
            }
        }
        else
        {
            Player.timeout -= Time.deltaTime * (float)Player.speed;
        }
    }

    private void Move()
    {
        if (RoomSpace.M.GetCell(this.x, this.y, 0) == 10)
        {
            Sounds.A.Play(5);
        }
        else
        {
            Sounds.A.Play(1);
        }
        base.GetComponent<Animation>().Play();
        int num = RoomSpace.M.Move(this.x, this.y, this.dx, this.dy, 1);
        if (num == 1)
        {
            this.Attack();
            return;
        }
        if (num == 2)
        {
            if (RoomSpace.door[RoomSpace.M.GetCell(this.x, this.y, 0)])
            {
                RoomSpace.M.NextRoom(base.GetComponent<Rotation>().rotation);
                return;
            }
            if (RoomSpace.M.GetCell(this.x, this.y, 0) == 13)
            {
                Sounds.A.Play(6);
                GameObject.Find("Win").GetComponent<Canvas>().enabled = true;
                UnityEngine.Object.Destroy(this);
            }
        }
    }

    private void Attack()
    {
        if (RoomSpace.M.GetCell(this.x + this.dx, this.y + this.dy, 1) > 0 && RoomSpace.movable[RoomSpace.M.GetCell(this.x + this.dx, this.y + this.dy, 1)])
        {
            Sounds.A.Play(3);
            RoomSpace.M.Move(this.x + this.dx, this.y + this.dy, this.dx, this.dy, 1);
        }
    }

    private void Death()
    {
        Sounds.A.Play(2);
        RoomSpace.M.keys[1] = 0;
        RoomSpace.M.PrintRoom(RoomSpace.roomid[0], RoomSpace.roomid[1]);
        Red.Pain();
    }
}
