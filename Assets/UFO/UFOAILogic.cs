using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UFOAILogic
{
    [SerializeField] float raycastDistance;
    [SerializeField] float raycastSphereRadius;
    private float hitDistance;
    private bool asteroidDetected;

    private Transform destination;
    private SpaceshipMovement movement;
    private Transform ownerTransform;

    public void Init(SpaceshipMovement movement, Transform ownerTransform)
    {
        this.movement = movement;
        this.ownerTransform = ownerTransform;
    }

    public void FixedUpdate()
    {
        FollowPlayer();
        float angle = AvoidAsteroids();
        Move(angle);
    }

    public void SetDestination(Transform point)
    {
        destination = point;
    }

    public void FollowPlayer()
    {
        var player = GameModeLogic.Instance.Player;
        if (player != null)
        {
            SetDestination(player.transform);
        }
        else
        {
            SetDestination(ownerTransform);
        }
    }

    private void Move(float angle)
    {
        if (asteroidDetected)
        {
            Vector3 direction = ownerTransform.rotation * Vector3.up;
            movement.MoveTo(direction);
            ownerTransform.Rotate(new Vector3(0, 0, angle));
        }
        else
        {
            Vector3 direction = destination.position - ownerTransform.position;
            movement.MoveTo(direction);
            ownerTransform.rotation = Quaternion.identity;
        }
    }

    private float AvoidAsteroids()
    {
        float result = 0f;
        RaycastHit hit;
        bool castHitted = Physics.SphereCast(ownerTransform.position, raycastSphereRadius, destination.position.normalized, out hit, raycastDistance);
        Asteroid target = null;
        if (castHitted)
        {
            target = hit.collider.GetComponent<Asteroid>();
        }
        if (castHitted && target != null)
        {
            hitDistance = hit.distance;
            if (!asteroidDetected)  // rotate only once
            {
                var asteroidVelocity = target.GetComponent<Rigidbody>().velocity;
                var ownerDirection = ownerTransform.up;
                float angle = Vector3.SignedAngle(asteroidVelocity, ownerDirection, -Vector3.forward);
                if (angle >= 0f && angle <= 180f)
                {
                    result = -45f;
                }
                else
                {
                    result = 45f;
                }
            }
            asteroidDetected = true;
        }
        else
        {
            asteroidDetected = false;
            result = 0f;
            hitDistance = raycastDistance;
        }

        return result;
    }

    public void DrawDebugInfo()
    {
        Debug.DrawRay(ownerTransform.position, destination.position.normalized * raycastDistance, Color.yellow);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(ownerTransform.position + destination.position.normalized * hitDistance, raycastSphereRadius);
    }
}
