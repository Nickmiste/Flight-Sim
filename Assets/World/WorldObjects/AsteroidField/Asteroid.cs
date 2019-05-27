using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour, IDamageable, ILockable
{
    float health = 6;

    void IDamageable.Damage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            ExplosionManager.CreateExplosion(transform.position);
            Destroy(gameObject);
        }
    }

    string ITargetable.GetTargetInfo()
    {
        return "";
    }
}
