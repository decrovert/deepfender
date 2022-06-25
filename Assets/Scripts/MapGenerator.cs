using System;
using UnityEngine;
using UnityEngine.Tilemaps;

using Random = UnityEngine.Random;

struct MapArray
{
    private byte[,] data;
    private byte width, height;

    public MapArray(byte width, byte height)
    {
        this.width = width;
        this.height = height;

        data = new byte[width, height];
    }

    public byte Width
    {
        get { return width; }
    }

    public byte Height
    {
        get { return height; }
    }

    public byte GetByte(byte x, byte y)
    {
        return data[x, y];
    }

    public void SetByte(byte x, byte y, byte value)
    {
        data[x, y] = value;
    }
}

struct Vector2Byte
{
    public byte x, y;

    public Vector2Byte(byte value)
    {
        x = y = value;
    }

    public Vector2Byte(byte x, byte y)
    {
        this.x = x;
        this.y = y;
    }
}

public class MapGenerator : MonoBehaviour
{
    [Header("Tiles")]
    [SerializeField] private TileBase pathTile = null;
    [SerializeField] private TileBase[] obstacleTiles = null;
    [SerializeField] private TileBase trashTile = null;

    private void GeneratePath(MapArray mapArray)
    {
        var pointer = new Vector2Byte(0, (byte)Random.Range(0, mapArray.Height));
        mapArray.SetByte(pointer.x, pointer.y, 1);

        while (pointer.x < mapArray.Width - 1)
        {
            // whether the pointer will move horizontally or vertically
            // true = x (horizontally)
            // false = y (vertically)
            var x_or_y = Random.Range(0, 2) > 0;

            if (x_or_y) // horizontal movement
            {
                ++pointer.x;
            }
            else // vertical movement
            {
                if (Random.Range(0, 2) > 0)
                {
                    if (pointer.y < mapArray.Height - 1)
                    {
                        ++pointer.y;
                    }
                }
                else
                {
                    if (pointer.y > 0)
                    {
                        --pointer.y;
                    }
                }
            }

            mapArray.SetByte(pointer.x, pointer.y, 1);
        }
    }

    /// <summary>
    /// Has a certain percent chance of returning true.
    /// </summary>
    /// <param name="chance"></param>
    /// <returns></returns>
    bool PercentChance(byte chance)
    {
        return Random.Range(0, 100) < chance - 1;
    }

    private void PopulateMapArray(MapArray mapArray)
    {
        for (byte x = 0; x < mapArray.Width; ++x)
        {
            for (byte y = 0; y < mapArray.Height; ++y)
            {
                var tileValue = mapArray.GetByte(x, y);

                if (tileValue != 0)
                {
                    continue;
                }

                if (PercentChance(20))
                {
                    mapArray.SetByte(x, y, (PercentChance(50)) ? (byte)2 : (byte)3);
                }
            }
        }
    }

    private MapArray GenerateMapArray(byte width, byte height)
    {
        var mapArray = new MapArray(width, height);
        GeneratePath(mapArray);
        PopulateMapArray(mapArray);

        return mapArray;
    }

    private Vector3Int MapArrayToCellPosition(Tilemap map, Vector2Byte mapArrayPosition)
    {
        var offset = new Vector3Int(map.origin.x, map.origin.y, 0);
        return new Vector3Int(mapArrayPosition.x, mapArrayPosition.y, 0) + offset;
    }

    private void WriteMapArrayToTilemap(MapArray mapArray, Tilemap tilemap)
    {
        for (byte x = 0; x < mapArray.Width; ++x)
        {
            for (byte y = 0; y < mapArray.Height; ++y)
            {
                var cellPosition = MapArrayToCellPosition(tilemap, new Vector2Byte(x, y));
                
                switch (mapArray.GetByte(x, y))
                {
                    case 1:
                        tilemap.SetTile(cellPosition, pathTile);
                        break;

                    case 2:
                        tilemap.SetTile(cellPosition, obstacleTiles[Random.Range(0, obstacleTiles.Length)]);
                        break;

                    case 3:
                        tilemap.SetTile(cellPosition, trashTile);
                        break;
                }
            }
        }
    }

    void Awake()
    {
        WriteMapArrayToTilemap(GenerateMapArray(16, 10), gameObject.GetComponent<Tilemap>());
    }
}
