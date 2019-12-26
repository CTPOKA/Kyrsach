using UnityEngine;

public class obj10 : MonoBehaviour
{
    int x, y;
    GameObject obj;
    void Start()
    {
        x = GetComponent<CellPos>().X;
        y = GetComponent<CellPos>().Y;
    }

    void Update()
    {
        if (!RoomSpace.intangible[RoomSpace.M.GetCell(x, y)] && (obj = RoomSpace.M.GetObject(x, y))!=null)
        {
            RoomSpace.M.Move(x, y, obj.GetComponent<CellPos>().dx, obj.GetComponent<CellPos>().dy);
        }
    }
}
