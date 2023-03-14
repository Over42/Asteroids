using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public BulletLogic BulletLogic { get => bulletLogic; }
    [SerializeField] BulletLogic bulletLogic;

    void Awake()
    {
        bulletLogic.Init(gameObject);
    }

    void Update()
    {
        BulletLogic.Update(Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        BulletLogic.Hit(other);
    }
}
