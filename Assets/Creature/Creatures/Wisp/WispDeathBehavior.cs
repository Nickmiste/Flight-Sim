using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WispDeathBehavior : DeathBehavior
{
    private const int deathTime = 50;

    private Vector3 baseScale;
    private Vector3 dir;

    public bool finalDeath = false;

    public WispDeathBehavior(Creature creature) : base(creature)
    {
        this.baseScale = creature.transform.localScale;
        this.dir = Random.onUnitSphere;
    }

    protected override void OnStart()
    {
        Wisp.wisps.Remove((Wisp)creature);
    }

    protected override bool ShouldDie()
    {
        //float scaleMod = finalDeath ? 7 : 1;
        //float speedMod = finalDeath ? 5 : 0.3f;
        //creature.transform.localScale = VectorUtil.Lerp3(baseScale, baseScale * scaleMod, Vector3.zero, 0.3f, (float) elapsed / deathTime);

        if (finalDeath)
        {
            creature.transform.localScale = VectorUtil.Lerp3(baseScale, baseScale * 5, Vector3.zero, 0.7f, (float)elapsed / deathTime);
            creature.transform.position += dir * 2;
        }
        else
        {
            creature.transform.localScale = Vector3.Lerp(baseScale, Vector3.zero, (float)elapsed / deathTime);
            creature.transform.position += dir * 0.3f;
        }

        return elapsed >= deathTime;
    }
}
