using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public void Triger(GameObject triger)
    {
        triger.SetActive(!triger.activeSelf);
    }
}
