using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class Enemy : MonoBehaviour
{
    private enum MoveDirection
    {
        Up = 0,
        Down,
        Forward
    }

    [Header("Tilemap")]
    [SerializeField] private TileBase pathTile;

    [Header("Attributes")]
    [SerializeField] private float speed = 4;

    [Header("Particles")]
    [SerializeField] private GameObject particleEmitter = null;

    private Tilemap tilemap;

    private MoveDirection currentDirection = MoveDirection.Forward;
    private Vector3 currentTargetPosition = Vector3.zero;
    private bool moving = false;

    public float health = 100;

    private EnemySpawner spawner = null;

    private void ChooseDirection()
    {
        List<MoveDirection> availableDirections = new List<MoveDirection>();

        var cellPosition = tilemap.WorldToCell(transform.position);

        var up = cellPosition + new Vector3Int(0, 1, 0);
        var down = cellPosition + new Vector3Int(0, -1, 0);
        var forward = cellPosition + new Vector3Int(-1, 0, 0);

        if (tilemap.GetTile(forward) == pathTile)
        {
            availableDirections.Add(MoveDirection.Forward);
        }

        if (tilemap.GetTile(up) == pathTile)
        {
            availableDirections.Add(MoveDirection.Up);
        }

        if (tilemap.GetTile(down) == pathTile)
        {
            availableDirections.Add(MoveDirection.Down);
        }

        currentDirection = availableDirections[Random.Range(0, availableDirections.Count)];

        switch (currentDirection)
        {
            case MoveDirection.Forward:
                currentTargetPosition = tilemap.GetCellCenterWorld(forward);
                break;

            case MoveDirection.Up:
                currentTargetPosition = tilemap.GetCellCenterWorld(up);
                break;
            
            case MoveDirection.Down:
                currentTargetPosition = tilemap.GetCellCenterWorld(down);
                break;
        }
    }

    private void Awake()
    {
        tilemap = GameObject.Find("MainTilemap").GetComponent<Tilemap>();
        spawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
    }

    private void Update()
    {
        if (health <= 0)
        {
            Instantiate(
                particleEmitter,
                new Vector3(transform.position.x, transform.position.y, -1),
                new Quaternion()
            );

            spawner.activeEnemies.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        if (moving)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                currentTargetPosition,
                Time.deltaTime * speed
            );

            if (transform.position == currentTargetPosition)
            {
                moving = false;
            }
        }
        else
        {
            if (tilemap.WorldToCell(transform.position).x == -8)
            {
                SceneManager.LoadScene("MainMenu");
            }

            ChooseDirection();
            moving = true;
        }
    }
}
