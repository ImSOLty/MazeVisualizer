using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private Animator anim;
    public bool open = false, discovered = false, path = false, expected = false;
    public int x, y;

    public List<Cell> expectedCells = new List<Cell>();

    void Start()
    {
        anim = GetComponent<Animator>();
    }


    public void SetOpen(bool type)
    {
        open = type;
        anim.SetBool("open", type);
    }

    public void SetDiscovered(bool type)
    {
        anim.SetBool("discovered", type);
    }
    public void SetExpected(bool type)
    {
        anim.SetBool("expected", type);
    }
    public void SetPath()
    {
        anim.SetBool("path", true);
    }
    public void SetDrop()
    {
        anim.SetBool("drop", true);
    }

    public void HighLight(bool type)
    {
        anim.SetBool("highlight", type);
    }

    public void SetXY(int xt, int yt)
    {
        x = xt;
        y = yt;
    }

    public void Pick()
    {
        anim.SetBool("pick", true);
    }
}