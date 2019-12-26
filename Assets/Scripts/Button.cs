using UnityEngine;

public class Button : MonoBehaviour
{
    public void Triger(GameObject triger)
    {
        triger.SetActive(!triger.active);
    }
}
