using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public SpawnerLogic Logic { get => logic; }
    [SerializeField] SpawnerLogic logic;

    void Awake()
    {
        logic.Init(this);
    }

    void Update()
    {
        logic.Update(Time.deltaTime);
    }
}
