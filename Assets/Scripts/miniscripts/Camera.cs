using UnityEngine;

public class Camera : MonoBehaviour
{
    private void Start()
    {
        transform.position = new Vector2(Editor.E.X * Editor.cellsizeX / 2, (Editor.E.Y + 1) * Editor.cellsizeY / 2);
    }
}
