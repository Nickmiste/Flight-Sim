using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultDeathBehavior : DeathBehavior
{
    public DefaultDeathBehavior(Creature creature) : base(creature)
    {

    }

    protected override bool ShouldDie()
    {
        return true;
    }
}
