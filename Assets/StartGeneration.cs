using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class StartGeneration : MonoBehaviour
{
    private bool generated = false;
    public GameObject prefab;

    public GameObject back;
    public GameObject text;
    public GameObject first, second, third, fourth, fifth;

    public List<Cell> discoverList = new List<Cell>();
    public List<Cell> finalList = new List<Cell>();
    public List<List<Cell>> ExpectedList = new List<List<Cell>>();
    
    
    int width = 35, height = 21;

    public Cell[,] cells;

    private void Start()
    {
        Application.targetFrameRate = 300;
    }

    void Update()
    {
        if (!generated && Input.GetKeyDown(KeyCode.Tab))
        {
            generated = true;
            StartCoroutine(Generate());
        }
    }

    int DFS(Cell[,] cells, Cell c)
    {
        c.discovered=true;
        discoverList.Add(c);
        if (c.path)
        {
            finalList.Add(c);
            return 1;
        }

        List<Cell> near = new List<Cell>();
        
        if (c.y + 1 < height && cells[c.x, c.y + 1].open && !cells[c.x, c.y + 1].discovered)
        {
            near.Add(cells[c.x, c.y + 1]);
        }

        if (c.y - 1 >= 0&& cells[c.x, c.y -1].open&& !cells[c.x, c.y -1].discovered)
        {
            near.Add(cells[c.x, c.y - 1]);
        }

        if (c.x + 1 < width && cells[c.x+1, c.y].open && !cells[c.x+1, c.y].discovered)
        {
            near.Add(cells[c.x + 1, c.y]);
        }

        if (c.x - 1 >= 0 && cells[c.x-1, c.y].open && !cells[c.x-1, c.y].discovered)
        {
            near.Add( cells[c.x - 1, c.y]);
        }

        if (near.Count > 1)
        {
            near[0].expectedCells = new List<Cell>(near);
        }
        foreach (var cell in near)
        {
            if (!cell.discovered && cell.open)
            {
                if (DFS(cells, cell) == 1)
                {
                    c.path = true;
                    finalList.Add(c);
                    return 1;
                }
            }
        }

        return 0;
    }
    
    IEnumerator Generate()
    {
        cells = new Cell[35, 21];

        
        
        // yield return new WaitForSeconds(1);
        //
        // first.SetActive(true);
        //
        // yield return new WaitForSeconds(1);
        //
        for (int j = height - 1; j >= 0; j--)
        {
            for (int i = 0; i < width; i++)
            {
                GameObject b = Instantiate(prefab, new Vector3((float) i / 3, (float) j / 3, 0), Quaternion.identity);
                cells[i, j] = b.GetComponent<Cell>();
                cells[i, j].SetXY(i, j);
                yield return null;
            }
        }
        yield return new WaitForSeconds(1f);
        // first.GetComponent<Animator>().SetBool("move", true);
        // yield return new WaitForSeconds(1f);
        //
        // second.SetActive(true);
        // yield return new WaitForSeconds(2f);
        // foreach (var cell in cells)
        // {
        //     cell.SetOpen(false);
        // }
        //
        // yield return new WaitForSeconds(2f);
        // foreach (var cell in cells)
        // {
        //     cell.SetOpen(true);
        // }
        //
        // yield return new WaitForSeconds(2f);
        // second.GetComponent<Animator>().SetBool("move", true);
        //
        // yield return new WaitForSeconds(1f);
        // third.SetActive(true);
        // yield return new WaitForSeconds(2f);
        foreach (var cell in cells)
        {
            cell.SetOpen(false);
        }
        //
        // yield return new WaitForSeconds(2f);
        // third.GetComponent<Animator>().SetBool("move", true);
        //
        // yield return new WaitForSeconds(1f);
        //
        // //lets pick a random
        // fourth.SetActive(true);
        yield return new WaitForSeconds(1f);
        
        int x = Random.Range(0, width);
        while (x % 2 != 1 || x < 4 || x > width - 4)
        {
            x = Random.Range(0, width);
        }
        
        int y = Random.Range(0, height);
        while (y % 2 != 1 || y < 4 || y > height - 4)
        {
            y = Random.Range(0, height);
        }
        //
        // cells[x,y].Pick();
        // yield return new WaitForSeconds(1);
        cells[x, y].SetOpen(true);

        yield return new WaitForSeconds(1);

        List<(Cell, Cell)> frontiers = new List<(Cell, Cell)>()
        {
            (cells[x - 2, y], cells[x - 1, y]),
            (cells[x + 2, y], cells[x + 1, y]),
            (cells[x, y - 2], cells[x, y - 1]),
            (cells[x, y + 2], cells[x, y + 1]),
        };
        // for (int i = 0; i < 2; i++)
        // {
        //     foreach (var cell in frontiers)
        //     {
        //         cell.Item1.HighLight(true);
        //     }
        //
        //     yield return new WaitForSeconds(0.5f);
        //
        //     foreach (var cell in frontiers)
        //     {
        //         cell.Item1.HighLight(false);
        //     }
        //     yield return new WaitForSeconds(0.5f);
        // }
        // foreach (var cell in frontiers)
        // {
        //     cell.Item1.HighLight(true);
        // }

        // yield return new WaitForSeconds(1f);
        // fourth.GetComponent<Animator>().SetBool("move", true);
        //
        // yield return new WaitForSeconds(1f);
        // fifth.SetActive(true);
        //Lets choose one of its frontiers
        int times = 200;
        while (true)
        {
            times++;
            if (times == 5)
            {
                yield return new WaitForSeconds(1f);
                fifth.GetComponent<Animator>().SetBool("move", true);
            }
            if (frontiers.Count == 0)
            {
                break;
            }

            (Cell, Cell) frontier = frontiers[Random.Range(0, frontiers.Count)];

            Cell ch = frontier.Item1;
            if (ch.open)
            {
                frontiers.Remove(frontier);
                continue;
            }

            ch.open = true;
            ch.Pick();

            if (times < 5)
                yield return new WaitForSeconds(1f);
            else
                yield return new WaitForSeconds(1/times*5);

            ch.SetOpen(true);

            if (times < 5)
                yield return new WaitForSeconds(0.5f);
            else
                yield return new WaitForSeconds(0.5f/times*5);

            frontier.Item2.SetOpen(true);

            if (times < 5)
                yield return new WaitForSeconds(0.5f);
            else
                yield return new WaitForSeconds(0.5f/times*5);

            int t = 0;

            List<(Cell, Cell)> newFrontiers = new List<(Cell, Cell)>();

            if (ch.y + 2 < height && !cells[ch.x, ch.y + 2].open)
            {
                t++;
                newFrontiers.Add((cells[ch.x, ch.y + 2], cells[ch.x, ch.y + 1]));
            }

            if (ch.y - 2 > 0 && !cells[ch.x, ch.y - 2].open)
            {
                t++;
                newFrontiers.Add((cells[ch.x, ch.y - 2], cells[ch.x, ch.y - 1]));
            }

            if (ch.x + 2 < width && !cells[ch.x + 2, ch.y].open)
            {
                t++;
                newFrontiers.Add((cells[ch.x + 2, ch.y], cells[ch.x + 1, ch.y]));
            }

            if (ch.x - 2 > 0 && !cells[ch.x - 2, ch.y].open)
            {
                t++;
                newFrontiers.Add((cells[ch.x - 2, ch.y], cells[ch.x - 1, ch.y]));
            }

            // foreach (var cell in newFrontiers)
            // {
            //     cell.Item1.HighLight(true);
            // }
            //
            // if (times < 5)
            //     yield return new WaitForSeconds(0.5f);
            // else
            //     yield return new WaitForSeconds(0.5f/times*5);
            //
            //
            // foreach (var cell in frontiers)
            // {
            //     cell.Item1.HighLight(true);
            // }
            //
            // if (times < 5)
            //     yield return new WaitForSeconds(0.5f);
            // else
            //     yield return new WaitForSeconds(0.5f/times*5);
            //
            frontiers.AddRange(newFrontiers);
            //
            // foreach (var cell in frontiers)
            // {
            //     cell.Item1.HighLight(false);
            // }

            if (times < 5)
                yield return new WaitForSeconds(0.5f);
            else
                yield return new WaitForSeconds(0.5f/times*5);


            Debug.Log(t);
            frontiers.Remove(frontier);
            if (frontiers.Count == 0)
            {
                break;
            }
        }
        Debug.Log("Finish");
        yield return new WaitForSeconds(1);
        cells[1, height-1].Pick();
        cells[width-2, 0].Pick();
        // yield return new WaitForSeconds(1);
        // cells[1, height-1].Pick();
        // cells[width-2, 0].Pick();
        yield return new WaitForSeconds(1);
        cells[1, height-1].SetOpen(true);
        cells[width-2, 0].SetOpen(true);

        cells[width-2, 0].path=true;
        
        DFS(cells, cells[1, height - 1]);

        Debug.Log("Found");

        int corner = 0;
        foreach (var c in discoverList)
        {
            if (c.expectedCells.Count!=0)
            {
                foreach(var c2 in c.expectedCells)
                {
                    c2.SetExpected(true);
                    yield return new WaitForSeconds(0.05f);
                }
            }
            c.SetDiscovered(true);
            yield return new WaitForSeconds(0.02f);
            if (c == cells[width - 2, 0])
            {
                break;
            }
        }
        yield return new WaitForSeconds(0.1f);
        Debug.Log(finalList.Count);
        foreach (var c in finalList)
        {
            c.SetPath();
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(1f);
        foreach (var c in cells)
        {
            if (!c.path)
            {
                c.SetExpected(false);
                c.SetDiscovered(false);
                c.SetDrop();
            }
            
        }
    }
}