using UnityEngine;

public class Healthbar : MonoBehaviour
{
    public float health = 25;

    void Update()
    {
        if (health > 100)
        {
            health = 100;
        }

        // health ---- 100
        // x --------- 8
        // x = health * 8 / 100
        this.transform.localScale = new Vector3((float)health * 8 / 100, 1, 1);
    }
}
