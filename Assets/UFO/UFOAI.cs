using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class UFOAI : MonoBehaviour
{
    public UFOAILogic Logic { get => logic; }
    [SerializeField] UFOAILogic logic;

    void Start()
    {
        var ufoMovement = GetComponent<UFO>().Movement;
        var ownerTransform = GetComponent<Transform>();
        Logic.Init(ufoMovement, ownerTransform);
    }

    void FixedUpdate()
    {
        Logic.FixedUpdate();
    }

    void OnDrawGizmosSelected()
    {
        logic.DrawDebugInfo();
    }
}
