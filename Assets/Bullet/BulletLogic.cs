using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BulletLogic
{
    private GameObject bullet;

    public event Action OnDestroyed;

    public float MaxLifetime { get => maxLifetime; }
    [SerializeField] float maxLifetime;
    [SerializeField] float speed;
    private float lifetime = 0.0f;

    public void Init(GameObject objectToMove)
    {
        bullet = objectToMove;
    }

    public void Update(float deltaTime)
    {
        bullet.transform.position += bullet.transform.up * speed * deltaTime;

        lifetime += deltaTime;
        if (lifetime >= maxLifetime)
        {
            OnDestroyed?.Invoke();
            lifetime = 0.0f;
        }
    }

    public void Hit(Collider collider)
    {
        var target = collider.GetComponent<IDamageable>();
        if (target != null)
        {
            target.TakeDamage();
            OnDestroyed?.Invoke();
        }
    }
}
