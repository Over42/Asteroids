using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpaceshipLogic: IDamageable
{
    public event Action OnDeath;

    public SelfDestructingVFX DestructionVFX { get => destructionVFX; }
    [SerializeField] SelfDestructingVFX destructionVFX;

    public void TakeDamage()
    {
        OnDeath.Invoke();
    }
}
