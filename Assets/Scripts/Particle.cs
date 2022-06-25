using UnityEngine;

public class Particle : MonoBehaviour
{
    void Awake()
    {
        var particleSys = GetComponent<ParticleSystem>();
        Destroy(gameObject, particleSys.main.duration);
    }
}
