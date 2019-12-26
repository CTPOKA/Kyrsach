using UnityEngine;

public class Edge : MonoBehaviour
{
    bool m = true;
    public float x1, x2, y1, y2;
    public GameObject point1=null, point2=null;
    void Update()
    {
        UpdatePos();
        transform.position = new Vector2(x1, y1);
        float a = Mathf.Atan2((y2 - y1),(x2 - x1));
        float b = Mathf.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1))-1;
        transform.rotation = Quaternion.AxisAngle(Vector3.forward, a - Mathf.PI/2);
        GetComponent<RectTransform>().sizeDelta = new Vector2(5,b);
    }
    public void UpdatePos()
    {
        if (point1!= null)
        {
            x1 = point1.transform.position.x;
            y1 = point1.transform.position.y;
        }
        if (point2 != null)
        {
            x2 = point2.transform.position.x;
            y2 = point2.transform.position.y;
            m = false;
        }
        else if (m)
        {
            x2 = Input.mousePosition.x;
            y2 = Input.mousePosition.y;
        }
        else Destroy(gameObject);
    }

    public void Point1(GameObject p)
    {
        point1 = p;
    }

    public void Point2(GameObject p)
    {
        point2 = p;
    }
}
