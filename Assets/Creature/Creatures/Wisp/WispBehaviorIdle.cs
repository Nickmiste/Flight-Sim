using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WispBehaviorIdle : CreatureBehavior
{
    public WispBehaviorIdle(Wisp wisp) : base(wisp)
    {

    }

    protected override void OnFixedUpdate()
    {
        float size = ((Wisp)creature).BaseScale.x + Mathf.Sin(Time.fixedTime * 5) * 0.5f;
        creature.transform.localScale = new Vector3(size, size, size);
    }
}
