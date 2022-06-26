using UnityEngine;
using UnityEngine.Tilemaps;

public class LaserTower : MonoBehaviour
{
    [Header("Tiles")]
    [SerializeField] private TileBase floorTile = null;
    [SerializeField] private TileBase laserTowerTile = null;

    [Header("Particles")]
    [SerializeField] private GameObject particleEmitter = null;

    private Tilemap mainTilemap = null;
    private Tilemap backgroundTilemap = null;

    private Camera cam = null;

    private GameManager gameManager = null;
    private Healthbar healthbar = null;

    private bool placed = false;

    void Awake()
    {
        mainTilemap = GameObject.Find("MainTilemap").GetComponent<Tilemap>();
        backgroundTilemap = GameObject.Find("BackgroundTilemap").GetComponent<Tilemap>();

        cam = GameObject.Find("Camera").GetComponent<Camera>();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        healthbar = GameObject.Find("Healthbar").GetComponent<Healthbar>();
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
}
