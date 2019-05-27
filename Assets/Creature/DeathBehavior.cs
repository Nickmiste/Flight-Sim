using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DeathBehavior : CreatureBehavior
{
    public DeathBehavior(Creature creature) : base(creature)
    {

    }

    protected sealed override void OnFixedUpdate()
    {
        if (ShouldDie())
        {
            OnDeath();
            GameObject.Destroy(creature.gameObject);
        }
    }

    protected abstract bool ShouldDie();
    protected virtual void OnDeath() { }
}
