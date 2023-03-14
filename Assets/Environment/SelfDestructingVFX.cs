using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructingVFX : MonoBehaviour
{
    private float maxLifetime;
    private float lifetime;

    void Awake()
    {
        var ps = GetComponent<ParticleSystem>();
        maxLifetime = ps.main.duration;
        ps.Play();
    }

    void Update()
    {
        lifetime += Time.deltaTime;
        if (lifetime >= maxLifetime)
            Destroy(gameObject);
    }
}
