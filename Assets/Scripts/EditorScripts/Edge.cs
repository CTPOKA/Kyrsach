using System;
using UnityEngine;

public class Edge : MonoBehaviour
{
    private bool m = true;

    public float x1;

    public float x2;

    public float y1;

    public float y2;

    public GameObject point1;

    public GameObject point2;

    [Obsolete]
    private void Update()
    {
        this.UpdatePos();
        base.transform.position = new Vector2(this.x1, this.y1);
        float num = Mathf.Atan2(this.y2 - this.y1, this.x2 - this.x1);
        float y = Mathf.Sqrt((this.x2 - this.x1) * (this.x2 - this.x1) + (this.y2 - this.y1) * (this.y2 - this.y1)) - 1f;
        base.transform.rotation = Quaternion.AxisAngle(Vector3.forward, num - 1.57079637f);
        base.GetComponent<RectTransform>().sizeDelta = new Vector2(5f, y);
    }

    public void UpdatePos()
    {
        if (this.point1 != null)
        {
            this.x1 = this.point1.transform.position.x;
            this.y1 = this.point1.transform.position.y;
        }
        if (this.point2 != null)
        {
            this.x2 = this.point2.transform.position.x;
            this.y2 = this.point2.transform.position.y;
            this.m = false;
            return;
        }
        if (this.m)
        {
            this.x2 = Input.mousePosition.x;
            this.y2 = Input.mousePosition.y;
            return;
        }
        UnityEngine.Object.Destroy(base.gameObject);
    }

    public void Point1(GameObject p)
    {
        this.point1 = p;
    }

    public void Point2(GameObject p)
    {
        this.point2 = p;
    }
}
