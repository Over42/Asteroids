using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpaceshipMovement
{
    private GameObject spaceship;

    [SerializeField] float acceleration;
    [SerializeField] float maxSpeed;
    [SerializeField] float rotationSpeed;

    public Vector2 Position { get => spaceship.transform.position; }
    public float Speed { get => currentVelocity.magnitude; private set { Speed = value; } }
    public float Rotation { get => spaceship.transform.rotation.eulerAngles.z; }

    private Vector3 currentVelocity;
    private float currentRotation;
    private int levelBoundaryX = 0;
    private int levelBoundaryY = 0;
    private float deltaTime;

    public void Init(GameObject objectToMove)
    {
        spaceship = objectToMove;
        var gameMode = GameModeLogic.Instance;
        SetLevelBoundaries(gameMode.LevelBoundaryX, gameMode.LevelBoundaryY);
    }

    public void Update(float deltaTime)
    {
        this.deltaTime = deltaTime;
        spaceship.transform.Translate(currentVelocity * deltaTime, Space.World);
        spaceship.transform.Rotate(new Vector3(0, 0, currentRotation * deltaTime));
        CheckExitScreen();
    }

    public void SetVelocityInput(float input)
    {
        float accelerationThisTick = acceleration * deltaTime * input;
        Vector3 directionThisTick = spaceship.transform.rotation * Vector3.up;
        CalculateVelocity(accelerationThisTick, directionThisTick);
    }

    public void SetRotationInput(float input)
    {
        currentRotation = rotationSpeed * -1f * input;
    }

    public void MoveTo(Vector3 destination)
    {
        float accelerationThisTick = acceleration * deltaTime;
        Vector3 directionThisTick = destination;
        CalculateVelocity(accelerationThisTick, directionThisTick);
    }

    private void CalculateVelocity(float acceleration, Vector3 direction)
    {
        Vector3 velocityThisTick = acceleration * direction;
        currentVelocity += velocityThisTick;

        if (currentVelocity.magnitude > maxSpeed)
        {
            currentVelocity = currentVelocity.normalized * maxSpeed;
        }
    }

    private void SetLevelBoundaries(int levelBoundaryX, int levelBoundaryY)
    {
        this.levelBoundaryX = levelBoundaryX;
        this.levelBoundaryY = levelBoundaryY;
    }

    private void CheckExitScreen()
    {
       if (GameModeLogic.Instance.IsInsideBoundaries(spaceship.transform.position))
          return;

        var loopedPosition = LoopPosition(spaceship.transform.position);
        spaceship.transform.position = loopedPosition;
    }

    private Vector2 LoopPosition(Vector3 position)
    {
        if (Mathf.Abs(position.x) > levelBoundaryX)
        {
            position = new Vector2(-Mathf.Sign(position.x) * levelBoundaryX, position.y);
        }

        if (Mathf.Abs(position.y) > levelBoundaryY)
        {
            position = new Vector2(position.x, -Mathf.Sign(position.y) * levelBoundaryY);
        }

        return position;
    }
}
