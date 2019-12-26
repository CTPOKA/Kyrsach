using UnityEngine;

public class Player : MonoBehaviour
{
    int x, y, dx, dy;
    public static float timeout;
    public static int speed = 5;
    void Update()
    {
        x = GetComponent<CellPos>().X;
        y = GetComponent<CellPos>().Y;
        try { if (RoomSpace.killing[RoomSpace.M.GetCell(x, y, 0)]) Death(); }
        catch
        {
            Destroy(this);
            Error.ErrorMassage("Ошибка расстановки. Уровень не правильно построен");
        }
        dx = 0; dy = 0;
        if (timeout < -0.5f)
        {
            if (Input.GetKey("down"))
            {
                dy = -1;
                GetComponent<Rotation>().rotation = Rotation.Down;
            }
            else if (Input.GetKey("up"))
            {
                dy = 1;
                GetComponent<Rotation>().rotation = Rotation.Up;
            }
            else if (Input.GetKey("left"))
            {
                dx = -1;
                GetComponent<Rotation>().rotation = Rotation.Left;
            }
            else if (Input.GetKey("right"))
            {
                dx = 1;
                GetComponent<Rotation>().rotation = Rotation.Right;
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                Death();
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log(RoomSpace.bigobject[2]);
            }
            if (dx != 0 || dy != 0)
            {
                timeout = 1;
                Move();
            }
        }
        else timeout -= Time.deltaTime * speed;
    }
    void Move()
    {
        if (RoomSpace.M.GetCell(x, y, 0) == 10) Sounds.A.Play(5);
        else Sounds.A.Play(1);
        GetComponent<Animation>().Play();
        int i = RoomSpace.M.Move(x, y, dx, dy);
        if (i == 1) Attack();
        else if (i == 2)
        {
            if (RoomSpace.door[RoomSpace.M.GetCell(x, y, 0)]) RoomSpace.M.NextRoom(GetComponent<Rotation>().rotation);
            else if (RoomSpace.M.GetCell(x, y, 0) == 13)
            {
                Sounds.A.Play(6);
                GameObject.Find("Win").GetComponent<Canvas>().enabled = true;
                Destroy(this);
            }
        }
    }
    void Attack()
    {
        if (RoomSpace.M.GetCell(x + dx, y + dy) > 0 && RoomSpace.movable[RoomSpace.M.GetCell(x + dx, y + dy)])
        {
            Sounds.A.Play(3);
            RoomSpace.M.Move(x + dx, y + dy, dx, dy);
        }
    }
    void Death()
    {
        Sounds.A.Play(2);
        RoomSpace.M.keys[1] = 0;
        RoomSpace.M.PrintRoom(RoomSpace.roomid[0], RoomSpace.roomid[1]);
        Red.Pain();
    }
}
