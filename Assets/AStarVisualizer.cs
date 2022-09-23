using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class AStarVisualizer : MonoBehaviour
{
    public GameObject tilePrefab;

    public void drawGrid(List<List<Tile>> grid)
    {
        foreach (var listTIles in grid)
        {
            foreach (var tile in listTIles)
            {
                if (tile.obj == null)
                    addGridSquare(tile, Color.white);
                else
                    setGridSquare(tile, Color.white);
            }
        }
    }

    public void clearGrid(List<List<Tile>> grid)
    {
        foreach (var listTiles in grid)
        {
            if (listTiles == null) continue;
            foreach (var tile in listTiles)
            {
                if (tile != null && tile.obj != null)
                        Destroy(tile.obj);
            }
        }
        
        grid.Clear();
    }

    public GameObject addGridSquare(Tile tile, Color color)
    {
        GameObject obj = Instantiate(tilePrefab);
        
        if (tile.active)
        {
            var child = obj.transform.GetChild(0);
            child.GetChild(0).GetComponent<TextMeshProUGUI>().text = tile.gCost + "";
            child.GetChild(1).GetComponent<TextMeshProUGUI>().text = tile.hCost + "";
            child.GetChild(2).GetComponent<TextMeshProUGUI>().text = tile.fCost + "";
        }
        
        obj.transform.position = (Vector3Int)tile.location;
        obj.GetComponent<SpriteRenderer>().color = color;
        tile.obj = obj;

        return obj;
    }

    public GameObject setGridSquare(Tile tile, Color color)
    {
        GameObject obj = tile.obj;
        
        if (tile.active)
        {
            var child = obj.transform.GetChild(0);
            child.GetChild(0).GetComponent<TextMeshProUGUI>().text = tile.gCost + "";
            child.GetChild(1).GetComponent<TextMeshProUGUI>().text = tile.hCost + "";
            child.GetChild(2).GetComponent<TextMeshProUGUI>().text = tile.fCost + "";
        }
        
        obj.transform.position = (Vector3Int)tile.location;
        obj.GetComponent<SpriteRenderer>().color = color;

        return obj;
    }

    public void setObstacles(List<Vector2Int> tileLocs, List<List<Tile>> grid)
    {
        foreach (var tileList in grid)
        {
            foreach (var tile in tileList.Where(a => a.obstical))
            {
                setGridSquare(tile, Color.white);
            }
        }
        
        foreach (var tileLoc in tileLocs)
        {
            Tile tile = grid[tileLoc.x][tileLoc.y];
            setObstacle(tile);
        }
    }

    public void setObstacle(Tile tile)
    {
        tile.obstical = true;
        setGridSquare(tile, Color.black);
    }
}
