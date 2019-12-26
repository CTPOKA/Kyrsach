using UnityEngine;

public class obj9 : MonoBehaviour
{
    int x, y;
    bool broken = false;
    void Start()
    {
        x = GetComponent<CellPos>().X;
        y = GetComponent<CellPos>().Y;
    }
    void Update()
    {
        if (Player.timeout < -0.1f)
        {
            if (!RoomSpace.intangible[RoomSpace.M.GetCell(x, y)]) broken = true;
            if (RoomSpace.intangible[RoomSpace.M.GetCell(x, y)] && broken)
            {
                Sounds.A.Play(3);
                RoomSpace.M.SetObject(x, y, 7, 0);
            }
        }
    }
}
