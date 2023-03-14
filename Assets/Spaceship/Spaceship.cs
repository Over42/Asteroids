using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour, IDamageable
{
    public SpaceshipLogic Logic { get => logic; }
    [SerializeField] SpaceshipLogic logic;
    public SpaceshipMovement Movement { get => movement; }
    [SerializeField] SpaceshipMovement movement;
    public SpaceshipWeapons Weapons { get => weapons; }
    [SerializeField] SpaceshipWeapons weapons;

    private SpaceshipController controller;

    private ParticleSystem engineTrailVFX;

    void Awake()
    {
        movement.Init(gameObject);
        weapons.Init();
        controller = GetComponent<SpaceshipController>();
        engineTrailVFX = GameObject.Find("EngineTrailVFX").GetComponent<ParticleSystem>();

        logic.OnDeath += () => Destroy(gameObject);
    }

    void Update()
    {
        movement.Update(Time.deltaTime);
        weapons.Update(Time.deltaTime);
        PlayEngineVFX();
    }

    public void TakeDamage()
    {
        Instantiate(logic.DestructionVFX, transform.position, Quaternion.identity);
        Logic.TakeDamage();
    }

    private void PlayEngineVFX()
    {
        if (controller.MoveInput > 0f && !engineTrailVFX.isPlaying)
            engineTrailVFX.Play();
        if (controller.MoveInput == 0f && engineTrailVFX.isPlaying)
            engineTrailVFX.Stop();
    }
}
