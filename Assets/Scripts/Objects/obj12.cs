using UnityEngine;
public class obj12 : MonoBehaviour
{
    int x, y;

    private void Start()
    {
        x = GetComponent<CellPos>().X;
        y = GetComponent<CellPos>().Y;
    }
    private void Update()
    {
        if (Player.timeout<0 && RoomSpace.M.GetCell(x, y) != 12)
        {
            Sounds.A.Play(4);
            RoomSpace.M.keys[1]++;
            Destroy(gameObject);
        }
    }
}
