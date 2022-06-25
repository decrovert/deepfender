using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [Header("Tilemaps")]
    [SerializeField] private Tilemap mainTilemap = null;
    [SerializeField] private Tilemap backgroundTilemap = null;

    [Header("EventTiles")]
    [SerializeField] private TileBase trashTile = null;

    [Header("Configuration")]
    [SerializeField] private GameObject particleEmitter = null;

    private GameObject healthbar = null;
    private Camera cam = null;

    void Awake()
    {
        healthbar = GameObject.Find("Healthbar");
        cam = GameObject.Find("Camera").GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mouseWorldPosition = cam.ScreenToWorldPoint(Input.mousePosition);
            var mousePositionInMainTilemap = mainTilemap.WorldToCell(mouseWorldPosition);

            if (mainTilemap.GetTile(mousePositionInMainTilemap) == trashTile)
            {
                mainTilemap.SetTile(mousePositionInMainTilemap, null);
                
                Instantiate(
                    particleEmitter,
                    new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, -1),
                    new Quaternion()
                );

                healthbar.GetComponent<Healthbar>().health += 5;
            }
        }
    }
}
