using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemies")]
    [SerializeField] private GameObject[] enemies = null;
    [SerializeField] private byte minimumSpawnTime = 2;
    [SerializeField] private byte maximumSpawnTime = 5;

    [Header("Tilemap")]
    [SerializeField] private Tilemap tilemap = null;
    [SerializeField] private TileBase pathTile = null;

    private Vector3? spawnPosition = null;
    private uint timer = 0;

    public List<GameObject> activeEnemies = new List<GameObject>();

    private void FindSpawnPosition()
    {
        for (var i = tilemap.cellBounds.min.y; i < tilemap.cellBounds.max.y; ++i)
        {
            if (tilemap.GetTile(new Vector3Int(tilemap.cellBounds.max.x - 1, i, 0)) == pathTile)
            {
                spawnPosition = tilemap.GetCellCenterWorld(new Vector3Int(
                    tilemap.cellBounds.max.x - 1,
                    i,
                    0
                ));
            }
        }
    }

    private void SpawnEnemy()
    {
        if (spawnPosition == null)
        {
            FindSpawnPosition();
        }

        activeEnemies.Add(Instantiate(
            enemies[Random.Range(0, enemies.Length)],
            (Vector3)spawnPosition,
            new Quaternion()
        ));
    }

    void FixedUpdate()
    {
        if (timer == 0)
        {
            timer = (uint)Random.Range(
                minimumSpawnTime * 60,
                maximumSpawnTime * 60
            );

            SpawnEnemy();
        }
        else
        {
            --timer;
        }
    }
}
