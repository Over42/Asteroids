using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour, IDamageable
{
    public AsteroidLogic Logic { get => logic; }
    [SerializeField] AsteroidLogic logic;

    void Awake()
    {
        logic.Init(this);
    }

    void OnCollisionEnter(Collision collision)
    {
        logic.CollisionEnter(collision);
    }

    public void TakeDamage()
    {
        Logic.TakeDamage();
        Destroy(gameObject);
    }
}
