using UnityEngine;
using UnityEngine.Tilemaps;

public class LaserTower : MonoBehaviour
{
    [Header("Tiles")]
    [SerializeField] private TileBase floorTile = null;
    [SerializeField] private TileBase laserTowerTile = null;
    [SerializeField] private TileBase trashTile = null;

    [Header("Particles")]
    [SerializeField] private GameObject particleEmitter = null;

    private Tilemap mainTilemap = null;
    private Tilemap backgroundTilemap = null;

    private Camera cam = null;

    private GameManager gameManager = null;
    private Healthbar healthbar = null;

    private bool placed = false;

    private EnemySpawner enemySpawner = null;
    private byte range = 3;

    [Header("Configuration")]
    [SerializeField] private byte damage = 150;

    private LineRenderer lineRenderer = null;

    private float health = 200;

    void Awake()
    {
        mainTilemap = GameObject.Find("MainTilemap").GetComponent<Tilemap>();
        backgroundTilemap = GameObject.Find("BackgroundTilemap").GetComponent<Tilemap>();

        cam = GameObject.Find("Camera").GetComponent<Camera>();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        healthbar = GameObject.Find("Healthbar").GetComponent<Healthbar>();

        enemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();

        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (!placed)
        {
            var mouseWorldPosition = cam.ScreenToWorldPoint(Input.mousePosition);

            transform.position = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, 0);

            if (Input.GetMouseButtonDown(0))
            {
                var mousePositionInMainTilemap = mainTilemap.WorldToCell(mouseWorldPosition);
                var mousePositionInBackgroundTilemap = backgroundTilemap.WorldToCell(mouseWorldPosition);

                if (backgroundTilemap.GetTile(mousePositionInBackgroundTilemap) == floorTile && mainTilemap.GetTile(mousePositionInMainTilemap) == null)
                {
                    transform.position = mainTilemap.GetCellCenterWorld(mousePositionInMainTilemap);
                    healthbar.health -= 20;
                    mainTilemap.SetTile(mousePositionInMainTilemap, laserTowerTile);
                    placed = true;

                    Instantiate(
                        particleEmitter,
                        new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, -1),
                        new Quaternion()
                    );

                    gameManager.shoppingMode = false;
                }
            }
        }
    }

    private GameObject GetClosestEnemy()
    {
        GameObject closestEnemy = null;
        float minimumDistance = Mathf.Infinity;

        foreach (var enemy in enemySpawner.activeEnemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance < minimumDistance)
            {
                closestEnemy = enemy;
                minimumDistance = distance;
            }
        }

        return closestEnemy;
    }

    private void FixedUpdate()
    {
        if (!placed)
        {
            return;
        }

        if (health <= 0)
        {
            mainTilemap.SetTile(mainTilemap.WorldToCell(transform.position), trashTile);

            Instantiate(
                particleEmitter,
                new Vector3(transform.position.x, transform.position.y, -1),
                new Quaternion()
            );

            Destroy(gameObject);
        }

        var closestEnemy = GetClosestEnemy();

        if (closestEnemy == null)
        {
            lineRenderer.enabled = false;
            return;
        }

        if (Vector3.Distance(transform.position, closestEnemy.transform.position) <= range)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, closestEnemy.transform.position);
            closestEnemy.GetComponent<Enemy>().health -= (float)damage / 60;
            health -= (float)damage / 60;
        }
    }
}
