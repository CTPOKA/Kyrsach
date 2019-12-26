using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class Node : MonoBehaviour, IPointerClickHandler
{
    static public GameObject medge;
    public GameObject edge;
    GameObject thisedge;
    public string nam;
    public int pos;
    public static string[] spawn = new string[2];
    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) Destroy(medge);
    }
    private void Start()
    {
        if (pos < 4)
        {
            nam = GetComponentInParent<Text>().text;
            if (FindDoor()==1) Destroy(gameObject);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Sounds.A.Play(0);
        if (medge != null)
        {
            DestroyEdge(thisedge);
            medge.GetComponent<Edge>().Point2(gameObject);
            thisedge = medge;
            medge = null;
            AddEdge();
        }
        else
        {
            DestroyEdge(thisedge);
            GameObject obj = Instantiate(edge) as GameObject;
            obj.transform.SetParent(transform.parent);
            obj.GetComponent<Edge>().Point1(gameObject);
            thisedge = obj;
            medge = obj;
        }
    }

    void AddEdge()
    {
        Node n = thisedge.GetComponent<Edge>().point1.GetComponent<Node>();
        if (pos < 4 && n.pos < 4)
        {
            int a = FloorEditor.names.IndexOf(n.nam);
            FloorEditor.map[a][n.pos] = nam;
            FloorEditor.map[a][n.pos + 4] = pos.ToString();
            a = FloorEditor.names.IndexOf(nam);
            FloorEditor.map[a][pos] = n.nam;
            FloorEditor.map[a][pos + 4] = n.pos.ToString();
        }
        else if (pos == 4)
        {
            spawn[0] = n.nam;
            spawn[1] = n.pos.ToString();
        }
        else if (n.pos == 4)
        {
            spawn[0] = nam;
            spawn[1] = pos.ToString();
        }
    }
    public void DestroyEdge(GameObject e)
    {
        if (e != null)
        {
            GameObject e1 = e.GetComponent<Edge>().point1;
            GameObject e2 = e.GetComponent<Edge>().point2;
            if (e1 != null && e2 != null)
            {
                Node n1 = e1.GetComponent<Node>();
                Node n2 = e2.GetComponent<Node>();
                if (n1.pos < 4 && n2.pos < 4)
                {
                    int a = FloorEditor.names.IndexOf(n1.nam);
                    FloorEditor.map[a][n1.pos] = "";
                    a = FloorEditor.names.IndexOf(n2.nam);
                    FloorEditor.map[a][n2.pos] = "";
                }
            }
            Destroy(e);
        }
    }
    int FindDoor()
    {
        string[] tmp = Crypt.Read("/Rooms/" + nam + ".ncs").Split('|');
        int n = 0;
        string[] str;
        str = tmp[n].Split(' '); n++;
        int x = int.Parse(str[0]), y = int.Parse(str[1]);
        for (int j = 0; j < 2; j++)
        {
            for (int yi = 0; yi < y; yi++)
            {
                str = tmp[n].Split(' '); n++;
                for (int xi = 0; xi < x; xi++)
                {
                    if (str[xi] == "5" || str[xi] == "11")
                    {
                        switch (pos)
                        {
                            case (0):
                                if (yi == y - 1) return 0;
                                break;
                            case (1):
                                if (yi == 0) return 0;
                                break;
                            case (3):
                                if (xi == 0) return 0;
                                break;
                            case (2):
                                if (xi == x - 1) return 0;
                                break;
                        }
                    }
                }
            }
        }
        return 1;
    }
}
