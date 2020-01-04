using UnityEngine;

public class Cell : MonoBehaviour
{
    public int lvl;

    public int x;

    public int y;

    private bool on = true;

    private SpriteRenderer sr;

    private void Awake()
    {
        this.sr = base.GetComponent<SpriteRenderer>();
    }

    public void CellInit(int x, int y, int lvl)
    {
        base.transform.position = new Vector3((float)x * 1.15f, (float)y * 0.78f, (float)y - 0.5f * (float)lvl);
        this.x = x;
        this.y = y;
        this.lvl = lvl;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Tab) && this.lvl == 1)
        {
            this.sr.enabled = false;
        }
        else
        {
            this.sr.enabled = true;
        }
        if (this.on)
        {
            this.sr.sprite = Editor.E.sprites[Editor.E.thisroom[this.lvl][this.x, this.y]];
            if (Editor.E.thisroom[this.lvl][this.x, this.y] == 0)
            {
                this.sr.color = new Color(0f, 0f, 0f, 0.1f);
            }
            else
            {
                this.sr.color = Color.white;
            }
        }
        if (Editor.E.lvl == this.lvl)
        {
            base.GetComponent<BoxCollider2D>().enabled = true;
            return;
        }
        base.GetComponent<BoxCollider2D>().enabled = false;
    }

    private void OnMouseExit()
    {
        this.on = true;
    }

    private void OnMouseOver()
    {
        this.on = false;
        if (!Editor.E.OFF[0] && !Editor.E.OFF[1] && !Editor.E.OFF[2])
        {
            if (Input.GetMouseButton(1))
            {
                this.sr.color = Color.red;
            }
            else if (Editor.E.type > 0)
            {
                this.sr.sprite = Editor.E.sprites[Editor.E.type];
                this.sr.color = Color.green;
            }
            if (Input.GetMouseButton(0))
            {
                if (Editor.E.type > 0)
                {
                    Sounds.A.Play(0);
                    Editor.E.thisroom[this.lvl][this.x, this.y] = Editor.E.type;
                }
            }
            else if (Input.GetMouseButton(1))
            {
                Sounds.A.Play(1);
                Editor.E.thisroom[this.lvl][this.x, this.y] = 0;
            }
            if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            {
                Editor.E.SaveBackup();
            }
        }
    }
}
