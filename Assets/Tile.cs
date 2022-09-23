using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Tile
{
    public bool active;
    public Vector2Int location;
    public int gCost;
    public int hCost;
    public int fCost;
    public Tile parent;
    
    public bool obstical;
    public bool startOrEnd;

    public GameObject obj;

    public Tile(Vector2Int location, int startingFCost = 0, int startingHCost = 0)
    {
        this.location = location;
        fCost = startingFCost;
        hCost = startingHCost;
    }

    public void setEndLocation(Vector2Int endLocation)
    {
        hCost = calcDist(location, endLocation);
        fCost = gCost + hCost;
    }

    public void activate(Tile parent, Vector2Int endLocation)
    {
        this.parent = parent;

        gCost = calcDist(location, parent.location) + parent.gCost;
        hCost = calcDist(location, endLocation);
        fCost = gCost + hCost;

        active = true;
    }

    private int calcDist(Vector2Int a, Vector2Int b)
    {
        return (int)(Math.Round(Vector2.Distance(a, b), 1) * 10);
    }
}
