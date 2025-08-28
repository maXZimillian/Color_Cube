using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLifeTime : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    [SerializeField] private ParticleSystemRenderer particles;
    void Start()
    {
        StartCoroutine(DestroyOnOverLifeTime());
        Material mat = new Material(particles.material);
        particles.material = mat;
    }

    private IEnumerator DestroyOnOverLifeTime()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
