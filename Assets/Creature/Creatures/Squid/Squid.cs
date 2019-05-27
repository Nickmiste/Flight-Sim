using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squid : Creature
{
    protected override float GetMaxHealth()
    {
        return 10;
    }

    protected override void OnStart()
    {
        AddBehavior(new SquidBehaviorWander(this));
    }

    protected override void OnFixedUpdate()
    {

    }
}