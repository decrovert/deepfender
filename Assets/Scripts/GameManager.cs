using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [Header("Tilemaps")]
    [SerializeField] private Tilemap mainTilemap = null;
    [SerializeField] private Tilemap backgroundTilemap = null;

    [Header("EventTiles")]
    [SerializeField] private TileBase trashTile = null;
    [SerializeField] private TileBase laserTowerTile = null;
    [SerializeField] private TileBase waterPurifierTile = null;

    [Header("Prefabs")]
    [SerializeField] private GameObject laserTowerPrefab = null;
    [SerializeField] private GameObject waterPurifierPrefab = null;

    [Header("Configuration")]
    [SerializeField] private GameObject particleEmitter = null;

    private Healthbar healthbar = null;
    private Camera cam = null;

    public bool shoppingMode = false;

    void Awake()
    {
        healthbar = GameObject.Find("Healthbar").GetComponent<Healthbar>();
        cam = GameObject.Find("Camera").GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }

        if (Input.GetMouseButtonDown(0))
        {
            var mouseWorldPosition = cam.ScreenToWorldPoint(Input.mousePosition);
            var mousePositionInMainTilemap = mainTilemap.WorldToCell(mouseWorldPosition);
            var mousePositionInBackgroundTilemap = backgroundTilemap.WorldToCell(mouseWorldPosition);

            if (shoppingMode)
            {
                return;
            }
            
            if (mainTilemap.GetTile(mousePositionInMainTilemap) == trashTile)
            {
                mainTilemap.SetTile(mousePositionInMainTilemap, null);

                Instantiate(
                    particleEmitter,
                    new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, -1),
                    new Quaternion()
                );

                healthbar.health += 5;

                return;
            }

            if (backgroundTilemap.GetTile(mousePositionInBackgroundTilemap) == laserTowerTile)
            {
                if (healthbar.health >= 20)
                {
                    shoppingMode = true;
                    Instantiate(
                        laserTowerPrefab,
                        new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, 0),
                        new Quaternion()
                    );
                }
            }
            else if (backgroundTilemap.GetTile(mousePositionInBackgroundTilemap) == waterPurifierTile)
            {
                if (healthbar.health >= 30)
                {
                    shoppingMode = true;
                    Instantiate(
                        waterPurifierPrefab,
                        new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, 0),
                        new Quaternion()
                    );
                }
            }
        }
    }
}
