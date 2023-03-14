using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UFOLogic : IDamageable
{
    [SerializeField] int winPoints;

    public void TriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damagable) && other.CompareTag("Player"))
        {
            damagable.TakeDamage();
        }
    }

    public void TakeDamage()
    {
        GameModeLogic.Instance.WinPoins += winPoints;
    }
}
