using UnityEngine;

public class Cell : MonoBehaviour
{
    public int lvl, x, y;
    bool on = true;
    SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    public void CellInit(int x, int y, int lvl)
    {
        transform.position = new Vector3(x * Editor.cellsizeX, y * Editor.cellsizeY, y - (0.5f*lvl));
        this.x = x;
        this.y = y;
        this.lvl = lvl;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Tab) && lvl==1)
        {
            sr.enabled = false;
        }
        else
        {
            sr.enabled = true;
        }
        if (on)
        {
            sr.sprite = Editor.E.sprites[Editor.E.thisroom[lvl][x, y]];
            if (Editor.E.thisroom[lvl][x, y] == 0) sr.color = new Color(0, 0, 0, 0.1f);
            else sr.color = Color.white;
        }
        if (Editor.E.lvl == lvl) GetComponent<BoxCollider2D>().enabled = true;
        else GetComponent<BoxCollider2D>().enabled = false;
    }
    private void OnMouseExit()
    {
        on = true;
    }

    private void OnMouseOver()
    {
        on = false;
        if (!Editor.E.OFF[0] && !Editor.E.OFF[1])
        {
            if (Input.GetMouseButton(1)) sr.color = Color.red;
            else
            {
                if (Editor.E.type > 0)
                {
                    sr.sprite = Editor.E.sprites[Editor.E.type];
                    sr.color = Color.green;
                }
            }
            if (Input.GetMouseButton(0))
            {
                if (Editor.E.type > 0)
                {
                    Sounds.A.Play(0);
                    Editor.E.thisroom[lvl][x, y] = Editor.E.type;
                }
            }
            else if (Input.GetMouseButton(1))
            {
                Sounds.A.Play(1);
                Editor.E.thisroom[lvl][x, y] = 0;
            }
            if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            {
                Editor.E.SaveBackup();
            }
        }
    }
}
