using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Seeker : MonoBehaviour
{
    public Grid grid;
    public AStarVisualizer aStarVisualizer;
    public Vector2Int gridSize;
    public Vector2Int startLoc;
    public Vector2Int endLoc;
    private Tile startTile;
    private Tile endTile;
    public int iterationCount;

    private void Start()
    {
        grid.update(gridSize);
        
        setStartEndTiles();
        seek(startTile, endTile);
    }
    
    public void OnValidate()
    {
        if (!Application.isPlaying) return;
        
        grid.update(gridSize);
        setStartEndTiles();
        
        StopCoroutine(seek(startTile, endTile));
        StartCoroutine(seek(startTile, endTile));
    }

    public void setStartEndTiles()
    {
        startTile = grid.tiles[startLoc.x][startLoc.y];
        startTile.startOrEnd = true;
        endTile = grid.tiles[endLoc.x][endLoc.y];
        endTile.startOrEnd = true;
        
        aStarVisualizer.setGridSquare(startTile, Color.green);
        aStarVisualizer.setGridSquare(endTile, Color.red);
    }

    public IEnumerator seek(Tile startTile, Tile endTile)
    {
        Tile currentTile = startTile;

        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();
        
        openList.Add(currentTile);

        for (int i = 0; i < 100000; i++)
        {
            iterationCount++;
            yield return new WaitForSeconds(0.0001f);
            openList = openList.OrderBy(a => a.fCost).ToList();
            currentTile = openList[0];

            openList.Remove(currentTile);
            closedList.Add(currentTile);

            if (currentTile.location == endTile.location)
                break;
            
            //Get ajacent tiles to current tile
            List<Tile> adjacentTiles = grid.getAdjacentTiles(currentTile);

            List<Tile> lowestCostTiles = new List<Tile>(){ new Tile(new Vector2Int(), Int32.MaxValue)};

            for (var j = 0; j < adjacentTiles.Count; j++)
            {
                var tile = adjacentTiles[j];
                if (closedList.Contains(tile))
                    continue;

                tile.activate(currentTile, endTile.location);
                aStarVisualizer.setGridSquare(tile, Color.gray);

                Tile matchingTile = openList.Find(a => a.location == tile.location);
                if (matchingTile != null && tile.gCost > matchingTile.gCost)
                    continue;
                
                openList.Add(tile);
                openList = openList.OrderBy(a => a.fCost).ToList();
            }

            foreach (var tile in closedList)
            {
                if (!tile.startOrEnd)
                    aStarVisualizer.setGridSquare(tile, new Color32(64, 64, 64, 255));
            }
        }

        Tile tracebackTile = currentTile;
        for (int i = 0; tracebackTile.location != startTile.location; i++)
        {
            yield return new WaitForEndOfFrame();
            if (i > 100000)
            {
                print("Failed to find the end after 200 tries");
                break;
            }

            tracebackTile = tracebackTile.parent;
            if (!tracebackTile.startOrEnd)
                aStarVisualizer.setGridSquare(tracebackTile, Color.blue);
        }
        
        setStartEndTiles();
        print("Finished " + currentTile.location);
    }
}
