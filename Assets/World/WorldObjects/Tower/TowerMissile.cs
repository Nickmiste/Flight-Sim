using System;
using UnityEngine;

public class TowerMissile : MonoBehaviour, ILockable
{
    [HideInInspector] public float damage;

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<IDamageable>()?.Damage(damage);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        ExplosionManager.CreateExplosion(transform.position, 0.3f);
    }

    string ITargetable.GetTargetInfo()
    {
        return "ignore";
    }
}