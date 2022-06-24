using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathGenerator : MonoBehaviour
{
    [SerializeField]
    private TileBase pathTile;

    private bool[,] GeneratePathArray(int width, int height)
    {
        bool[,] path = new bool[width, height];
        Vector2Int pointer = new Vector2Int(0, Random.Range(0, height));
        path[pointer.x, pointer.y] = true;

        while (pointer.x < width - 1)
        {
            // Whether the pointer will move horizontally or vertically
            // true = x (horizontally)
            // false = y (vertically)
            bool x_or_y = Random.Range(0, 2) > 0;

            if (x_or_y) // horizontal movement
            {
                ++pointer.x;
            }
            else // vertical movement
            {
                if (Random.Range(0, 2) > 0)
                {
                    if (pointer.y < height - 1 && path[pointer.x, pointer.y + 1] != true)
                    {
                        ++pointer.y;
                    }
                }
                else
                {
                    if (pointer.y > 0 && path[pointer.x, pointer.y - 1] != true)
                    {
                        --pointer.y;
                    }
                }
            }

            path[pointer.x, pointer.y] = true;
        }

        return path;
    }

    private Vector3Int PathArrayToCellPosition(Tilemap map, Vector3Int pathArrayPosition)
    {
        var offset = new Vector3Int(map.origin.x, map.origin.y, 0);
        return pathArrayPosition + offset;
    }

    private void WritePathArrayToMap(bool[,] path, Tilemap map, TileBase pathTile)
    {
        for (int x = 0; x < path.GetLength(0); ++x)
        {
            for (int y = 0; y < path.GetLength(1); ++y)
            {
                var cellPosition = PathArrayToCellPosition(map, new Vector3Int(x, y, 0));
                
                if (path[x, y] == true)
                {
                    map.SetTile(cellPosition, pathTile);
                }
            }
        }
    }

    void Start()
    {
        WritePathArrayToMap(GeneratePathArray(16, 10), this.gameObject.GetComponent<Tilemap>(), this.pathTile);
    }
}
