using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Node : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
    public static GameObject medge;

    public GameObject edge;

    private GameObject thisedge;

    public string nam;

    public int pos;

    public static string[] spawn = new string[2];

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            UnityEngine.Object.Destroy(Node.medge);
        }
    }

    private void Start()
    {
        if (this.pos < 4)
        {
            this.nam = base.GetComponentInParent<Text>().text;
            if (this.FindDoor() == 1)
            {
                UnityEngine.Object.Destroy(base.gameObject);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Sounds.A.Play(0);
        if (Node.medge != null)
        {
            this.DestroyEdge(this.thisedge);
            Node.medge.GetComponent<Edge>().Point2(base.gameObject);
            this.thisedge = Node.medge;
            Node.medge = null;
            this.AddEdge();
            return;
        }
        this.DestroyEdge(this.thisedge);
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.edge);
        gameObject.transform.SetParent(base.transform.parent);
        gameObject.GetComponent<Edge>().Point1(base.gameObject);
        this.thisedge = gameObject;
        Node.medge = gameObject;
    }

    private void AddEdge()
    {
        Node component = this.thisedge.GetComponent<Edge>().point1.GetComponent<Node>();
        if (this.pos < 4 && component.pos < 4)
        {
            int index = FloorEditor.names.IndexOf(component.nam);
            FloorEditor.map[index][component.pos] = this.nam;
            FloorEditor.map[index][component.pos + 4] = this.pos.ToString();
            index = FloorEditor.names.IndexOf(this.nam);
            FloorEditor.map[index][this.pos] = component.nam;
            FloorEditor.map[index][this.pos + 4] = component.pos.ToString();
            return;
        }
        if (this.pos == 4)
        {
            Node.spawn[0] = component.nam;
            Node.spawn[1] = component.pos.ToString();
            return;
        }
        if (component.pos == 4)
        {
            Node.spawn[0] = this.nam;
            Node.spawn[1] = this.pos.ToString();
        }
    }

    public void DestroyEdge(GameObject e)
    {
        if (e != null)
        {
            GameObject point = e.GetComponent<Edge>().point1;
            GameObject point2 = e.GetComponent<Edge>().point2;
            if (point != null && point2 != null)
            {
                Node component = point.GetComponent<Node>();
                Node component2 = point2.GetComponent<Node>();
                if (component.pos < 4 && component2.pos < 4)
                {
                    int index = FloorEditor.names.IndexOf(component.nam);
                    FloorEditor.map[index][component.pos] = "";
                    index = FloorEditor.names.IndexOf(component2.nam);
                    FloorEditor.map[index][component2.pos] = "";
                }
            }
            UnityEngine.Object.Destroy(e);
        }
    }

    private int FindDoor()
    {
        string[] array = Crypt.Read("/Rooms/" + this.nam + ".ncs").Split(new char[]
        {
            '|'
        });
        int num = 0;
        string[] array2 = array[num].Split(new char[]
        {
            ' '
        });
        num++;
        int num2 = int.Parse(array2[0]);
        int num3 = int.Parse(array2[1]);
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < num3; j++)
            {
                array2 = array[num].Split(new char[]
                {
                    ' '
                });
                num++;
                for (int k = 0; k < num2; k++)
                {
                    if (array2[k] == "5" || array2[k] == "11")
                    {
                        switch (this.pos)
                        {
                            case 0:
                                if (j == num3 - 1)
                                {
                                    return 0;
                                }
                                break;
                            case 1:
                                if (j == 0)
                                {
                                    return 0;
                                }
                                break;
                            case 2:
                                if (k == num2 - 1)
                                {
                                    return 0;
                                }
                                break;
                            case 3:
                                if (k == 0)
                                {
                                    return 0;
                                }
                                break;
                        }
                    }
                }
            }
        }
        return 1;
    }
}
