using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class SpaceshipWeapons
{
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] float bulletCooldown;
    private float bulletShootTime;
    private BulletPool pool;

    public int LaserCharges { get; private set; }
    [SerializeField] int maxLaserCharges;
    public float CurrentLaserRestoreCooldown { get; private set; }
    [SerializeField] float laserRestoreCooldown;
    [SerializeField] float laserShootCooldown;
    [SerializeField] float laserRange;
    [SerializeField] float laserDuration;
    private float laserShootTime;
    private LineRenderer laserLine;

    public void Init()
    {
        CreateBulletPool();

        laserLine = GameObject.Find("BulletSpawnPoint").GetComponent<LineRenderer>();
        LaserCharges = maxLaserCharges;
        CurrentLaserRestoreCooldown = laserRestoreCooldown;
    }

    public void Update(float deltaTime)
    {
        RestoreLaser(deltaTime);
    }

    private void CreateBulletPool()
    {
        int poolSize = Mathf.CeilToInt(bulletPrefab.BulletLogic.MaxLifetime / bulletCooldown) + 1;
        pool = new BulletPool(bulletPrefab, bulletSpawnPoint, poolSize);
    }
    
    public void ShootBullet()
    {
        if (Time.time - bulletShootTime > bulletCooldown)
        {
            var bullet = pool.Get();
            if (bullet != null)
                bullet.BulletLogic.OnDestroyed += () => pool.Return(bullet);
            bulletShootTime = Time.time;
        }
    }

    public void ShootLser()
    {
        if (Time.time - laserShootTime > laserShootCooldown && LaserCharges > 0)
        {
            Vector3 rayOrigin = bulletSpawnPoint.position;
            Vector3 rayDirection = bulletSpawnPoint.up;
            laserLine.SetPosition(0, rayOrigin);
            var ray = new Ray(rayOrigin, rayDirection);
            RaycastHit[] hits = Physics.RaycastAll(ray, laserRange);
            foreach(RaycastHit obj in hits)
            {
                var target = obj.collider.GetComponent<IDamageable>();
                if (target != null)
                {
                    target.TakeDamage();
                }
            }
            laserLine.SetPosition(1, rayOrigin + (rayDirection * laserRange));

            LaserCharges--;
            laserShootTime = Time.time;
            DisplayLaser();
        }
    }

    private async void DisplayLaser()
    {
        laserLine.enabled = true;
        await Task.Delay((int)(laserDuration * 1000f));
        if (laserLine)
            laserLine.enabled = false;
    }

    private void RestoreLaser(float deltaTime)
    {
        if (LaserCharges < maxLaserCharges)
        {
            CurrentLaserRestoreCooldown -= deltaTime;
            CurrentLaserRestoreCooldown = Mathf.Max(0f, CurrentLaserRestoreCooldown);
        }

        if (CurrentLaserRestoreCooldown == 0)
        {
            LaserCharges++;
            CurrentLaserRestoreCooldown = laserRestoreCooldown;
        }
    }
}

public class BulletPool
{
    private List<Bullet> pooledObjects;
    private int poolSize = 10;
    private Transform bulletsSpawnPoint;

    public BulletPool(Bullet objectToPool, Transform bulletSpawnPoint, int poolSize)
    {
        pooledObjects = new List<Bullet>();
        this.poolSize = poolSize;
        bulletsSpawnPoint = bulletSpawnPoint;
        Bullet bullet;
        for (int i = 0; i < this.poolSize; i++)
        {
            bullet = GameObject.Instantiate(objectToPool, bulletsSpawnPoint.position, bulletsSpawnPoint.rotation);
            bullet.gameObject.SetActive(false);
            pooledObjects.Add(bullet);
        }
    }

    public Bullet Get()
    {
        for (int i = 0; i < poolSize; i++)
        {
            if (!pooledObjects[i].gameObject.activeInHierarchy)
            {
                var bullet = pooledObjects[i];
                bullet.transform.position = bulletsSpawnPoint.position;
                bullet.transform.rotation = bulletsSpawnPoint.rotation;
                bullet.gameObject.SetActive(true);
                return bullet;
            }
        }
        return null;
    }

    public void Return(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }
}
