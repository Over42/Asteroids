using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AsteroidLogic: IDamageable
{
    private Asteroid asteroid;

    [SerializeField] bool isBig;
    [SerializeField] int winPoints;
    [SerializeField] Asteroid smallAsteroidPrefab;
    [SerializeField] int minAsteroidFragments;
    [SerializeField] int maxAsteroidFragments;

    public void Init(Asteroid asteroid)
    {
        this.asteroid = asteroid;

        if (isBig)
        {
            Vector2 force = asteroid.transform.position.normalized * -1000.0f;
            Vector3 torque = Random.insideUnitSphere * Random.Range(500.0f, 1500.0f);
            Spin(force, torque);
        }
    }

    public void CollisionEnter(Collision collision)
    {
        var collidedWith = collision.gameObject;
        if (collidedWith.TryGetComponent(out IDamageable damagable) && collidedWith.CompareTag("Player"))
        {
            damagable.TakeDamage();
        }
    }

    public void TakeDamage()
    {
        if (isBig)
        {
            BreakUpBigAsteroid(asteroid.transform.position);
        }
        GameModeLogic.Instance.WinPoins += winPoints;
    }

    public void BreakUpBigAsteroid(Vector2 position)
    {
        int fragmentsToSpawn = Random.Range(minAsteroidFragments, maxAsteroidFragments);
        for (int counter = 0; counter < fragmentsToSpawn; ++counter)
        {
            Vector2 force = Quaternion.Euler(0, 0, counter * 360.0f / fragmentsToSpawn) * Vector2.up * Random.Range(0.5f, 1.5f) * 300.0f;
            Vector3 torque = Random.insideUnitSphere * Random.Range(500.0f, 1500.0f);
            Quaternion rotation = Quaternion.Euler(0, 0, Random.value * 180.0f);

            var asteroid = GameObject.Instantiate(smallAsteroidPrefab, position + force.normalized * 10.0f, rotation);
            asteroid.Logic.Spin(force, torque);
        }
    }

    public void Spin(Vector2 force, Vector3 torque)
    {
        var rb = asteroid.GetComponent<Rigidbody>();
        rb.AddForce(force);
        rb.AddTorque(torque);
    }
}
