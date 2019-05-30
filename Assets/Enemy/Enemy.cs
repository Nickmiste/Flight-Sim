using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovementBase, ILockable, IDamageable
{
    [Header("Enemy Settings")]
    [SerializeField] private float maxHull = 10;
    public float hull { get; private set; }

    private void Start()
    {
        hull = maxHull;
    }

    private void FixedUpdate()
    {
        throttle = 1f;
        Vector3 normalPlayerOffset = (PlayerUtil.GetPosition() - transform.position).normalized;
        roll = -Vector3.Dot(normalPlayerOffset, transform.right);
        pitch = -Vector3.Dot(normalPlayerOffset, transform.up);

        DoMovement();
    }

    void IDamageable.Damage(float damage)
    {
        hull -= damage;
        if (hull <= 0)
        {
            ExplosionManager.CreateExplosion(transform.position);
            Destroy(gameObject);
        }
    }

    string ITargetable.GetTargetInfo()
    {
        return string.Format("{0}/{1} hull", hull, maxHull);
    }
}
