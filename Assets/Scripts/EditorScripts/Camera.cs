using UnityEngine;

public class Camera : MonoBehaviour
{
    private void Start()
    {
        base.transform.position = new Vector2((float)Editor.E.X * 1.15f / 2f, (float)(Editor.E.Y + 1) * 0.78f / 2f);
    }
}