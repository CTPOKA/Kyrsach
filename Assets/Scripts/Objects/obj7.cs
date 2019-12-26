using UnityEngine;

public class obj7 : MonoBehaviour
{
    int x, y;
    public Sprite s;
    void Start()
    {
        x = GetComponent<CellPos>().X;
        y = GetComponent<CellPos>().Y;
    }
    void Update()
    {
        if (Player.timeout <= 0)
        {
            if (RoomSpace.M.GetCell(x, y) == 4)
            {
                RoomSpace.M.SetObject(x, y, 0);
                RoomSpace.M.SetObject(x, y, 1, 0).GetComponent<SpriteRenderer>().sprite = s;
            }
        }
    }
}
