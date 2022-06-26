using UnityEngine;
using UnityEngine.Tilemaps;

public class WaterPurifier : MonoBehaviour
{
    [Header("Tiles")]
    [SerializeField] private TileBase floorTile = null;
    [SerializeField] private TileBase waterPurifierTile = null;
    [SerializeField] private TileBase trashTile = null;

    [Header("Particles")]
    [SerializeField] private GameObject particleEmitter = null;

    private Tilemap mainTilemap = null;
    private Tilemap backgroundTilemap = null;

    private Camera cam = null;

    private GameManager gameManager = null;
    private Healthbar healthbar = null;

    private bool placed = false;
    private float health = 50;

    [Header("Configuration")]
    [SerializeField] private byte cleaningRate = 1;

    private void Awake()
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
                    healthbar.health -= 30;
                    mainTilemap.SetTile(mousePositionInMainTilemap, waterPurifierTile);
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

        healthbar.health += (float)cleaningRate / 60;
        health -= (float)cleaningRate / 2 / 60;
    }
}
