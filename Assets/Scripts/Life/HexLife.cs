using System.Collections.Generic;
using UnityEngine;

public class HexLife : MonoBehaviour
{
    public HexGrid grid;

    private HashSet<Hex> alive = new HashSet<Hex>();
    private HashSet<Hex> newAlive = new HashSet<Hex>();
    private HashSet<Hex> visitable = new HashSet<Hex>();

    public void ToggleCell(Hex hex)
    {
        if (alive.Contains(hex))
            alive.Remove(hex);
        else
            alive.Add(hex);

        UpdateDisplay();
    }

    void Start()
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        foreach (var kv in grid.Map)
        {
            bool isAlive = alive.Contains(kv.Key);
            kv.Value.SetAlive(isAlive);
        }
    }

    public void Step()
    {
        newAlive.Clear();
        BuildVisitables();

        foreach (Hex h in visitable)
        {
            int count = CountAliveNeighbors(h);
            
            // Hex game of life rules from https://arunarjunakani.github.io/HexagonalGameOfLife/
            if (count == 2)
                newAlive.Add(h);
        }

        (alive, newAlive) = (newAlive, alive);
        UpdateDisplay();
    }

    void BuildVisitables()
    {
        visitable.Clear();
        foreach (Hex h in alive)
        {
            visitable.Add(h);
            foreach (Hex neighbor in h.neighbors())
                visitable.Add(neighbor);
        }
    }

    int CountAliveNeighbors(Hex hex)
    {
        int count = 0;
        foreach (Hex neighbor in hex.neighbors())
        {
            if (alive.Contains(neighbor))
                count++;
        }
        return count;
    }

    public void Clear()
    {
        alive.Clear();
        UpdateDisplay();
    }
}