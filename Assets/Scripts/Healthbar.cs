using UnityEngine;

public class Healthbar : MonoBehaviour
{
    [Range(0, 100)]
    public byte health = 25;

    void Update()
    {
        // health ---- 100
        // x --------- 8
        // x = health * 8 / 100
        this.transform.localScale = new Vector3((float)health * 8 / 100, 1, 1);
    }
}
