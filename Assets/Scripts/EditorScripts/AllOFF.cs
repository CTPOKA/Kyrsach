using UnityEngine;

public class AllOFF : MonoBehaviour
{
    int on = 0;
    private void OnDisable()
    {
        on++;
        Editor.E.AllOFF(on>1);
    }
}