using UnityEngine;
using System.Collections;

public class GameOfLife : MonoBehaviour
{
    public int width = 30;
    public int height = 30;

    public GameObject cellPrefab;
    private Cell[,] grid;
    private bool[,] nextState;

    private void Start()
    {
        grid = new Cell[width, height];
        nextState = new bool[width, height];
        GenerateGrid();
        StartCoroutine(SimulationLoop());
    }

    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject obj = Instantiate(cellPrefab);
                obj.transform.position = new Vector3(x, 0, y);
                obj.transform.parent = this.transform;

                Cell cell = obj.GetComponent<Cell>();
                bool alive = Random.value > 0.7;
                cell.SetState(alive);
                grid[x, y] = cell;
            }
        }

    }
    int CountNeighbors(int x, int y)
    {
        int count = 0;
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0)
                {
                    continue;
                }
                int nx = x + dx;
                int ny = y + dy;
                if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                {
                    if (grid[nx, ny].isAlive)
                    {
                        count++;
                    }
                }

            }
        }
        return count;
    }

    void ApplyNextState()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y].SetState(nextState[x, y]);
            }
        }
    }
    void Simulate()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbors = CountNeighbors(x, y);

                bool alive = grid[x, y].isAlive;
                if (alive)
                {
                    nextState[x, y] = neighbors == 2 || neighbors == 3;
                }
                else
                {
                    nextState[x, y] = neighbors == 3;
                }
            }
        }
        ApplyNextState();
    }
    IEnumerator SimulationLoop()
    {
        while (true) {
            Simulate();
            yield return new WaitForSeconds(0.27f);
        }
    }
}
