using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour, IDamageable
{
    public UFOLogic Logic { get => logic; }
    [SerializeField] UFOLogic logic;

    public SpaceshipMovement Movement { get => movement; }
    [SerializeField] SpaceshipMovement movement;

    void Awake()
    {
        movement.Init(gameObject);
    }

    void Update()
    {
        movement.Update(Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        logic.TriggerEnter(other);
    }

    public void TakeDamage()
    {
        Logic.TakeDamage();
        Destroy(gameObject);
    }
}
