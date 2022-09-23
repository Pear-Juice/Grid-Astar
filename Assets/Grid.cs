using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public AStarVisualizer aStarVisualizer;
    public List<List<Tile>> tiles = new List<List<Tile>>();
    public Vector2Int endLocation;
    public GameObject tilePrefab;
    public List<Vector2Int> obstacles = new List<Vector2Int>();

    private void OnValidate()
    {
        if (Application.isPlaying)
            aStarVisualizer.setObstacles(obstacles, tiles);
    }

    public void update(Vector2Int size)
    {
        aStarVisualizer.clearGrid(tiles);

        for (int i = 0; i < size.x; i++)
        {
            tiles.Add(new List<Tile>());

            for (int j = 0; j < size.y; j++)
            {
                tiles[i].Add(new Tile(new Vector2Int(i, j)));
            }
        }

        aStarVisualizer.drawGrid(tiles);

        aStarVisualizer.setObstacles(obstacles, tiles);
    }

    public void setGridEndLocation(Vector2Int endLocation)
    {
        this.endLocation = endLocation;
        
        foreach (var tileRow in tiles)
        {
            foreach (var tileCol in tileRow)
            {
                tileCol.setEndLocation(endLocation);
            }
        }
    }

    public Tile getTile(Vector2Int loc)
    {
        return tiles[loc.x][loc.y];
    }

    public List<Tile> getAdjacentTiles(Tile tile)
    {
        Vector2Int loc = tile.location;
        
        List<Tile> adjacentTiles = new List<Tile>();
        
        for (int i = loc.x - 1; i < loc.x + 2; i++)
        {
            for (int j = loc.y - 1; j < loc.y + 2; j++)
            {
                if (i >= 0 && i < tiles.Count)
                    if (j >= 0 && j < tiles[i].Count)
                        if (!(i == loc.x && j == loc.y))
                            if (tiles[i] != null && !tiles[i][j].obstical)
                                adjacentTiles.Add(tiles[i][j]);
            }
        }

        return adjacentTiles;
    }
}
