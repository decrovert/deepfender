using System.Collections.Generic;
using UnityEngine;
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

    private Tilemap tilemap;

    private MoveDirection currentDirection = MoveDirection.Forward;
    private Vector3 currentTargetPosition = Vector3.zero;
    private bool moving = false;

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
            ChooseDirection();
            moving = true;
        }
    }
}
