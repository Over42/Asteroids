using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    private Spaceship spaceship;

    [SerializeField] SpaceshipHUD spaceshipHUD;

    public float MoveInput { get; private set; }

    private void Awake()
    {
        spaceship = GetComponent<Spaceship>();

        var hud = Instantiate(spaceshipHUD);
        spaceship.Logic.OnDeath += () => Destroy(hud.gameObject);
    }

    void Update()
    {
        float verticalAxis = Input.GetAxis("Vertical");
        MoveInput = verticalAxis;
        if (verticalAxis >= 0)
        {
            spaceship.Movement.SetVelocityInput(verticalAxis);
        }

        float horizontalAxis = Input.GetAxis("Horizontal");
        if (horizontalAxis != 0)
        {
            spaceship.Movement.SetRotationInput(horizontalAxis);
        }

        if (Input.GetButton("Fire1"))
        {
            spaceship.Weapons.ShootBullet();
        }

        if (Input.GetButton("Fire2"))
        {
            spaceship.Weapons.ShootLser();
        }
    }
}
