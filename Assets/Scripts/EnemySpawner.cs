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

    private uint timer = 0;

    private void SpawnEnemy()
    {
        for (var i = tilemap.cellBounds.min.y; i < tilemap.cellBounds.max.y; ++i)
        {
            if (tilemap.GetTile(new Vector3Int(tilemap.cellBounds.max.x - 1, i, 0)) == pathTile)
            {
                Instantiate(
                    enemies[Random.Range(0, enemies.Length)],
                    tilemap.GetCellCenterWorld(new Vector3Int(tilemap.cellBounds.max.x - 1, i, 0)),
                    new Quaternion()
                );
            }
        }
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
