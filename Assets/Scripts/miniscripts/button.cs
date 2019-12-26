using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button : MonoBehaviour
{
    public void Triger(GameObject triger)
    {
        triger.SetActive(!triger.active);
    }
}
