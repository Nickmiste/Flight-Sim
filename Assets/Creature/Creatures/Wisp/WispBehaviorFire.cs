using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WispBehaviorFire : CreatureBehavior
{
    private const int duration = 50;
    private Vector3 beamMeetPoint;

    public WispBehaviorFire(Wisp wisp, float beamMeetDistance) : base(wisp)
    {
        this.beamMeetPoint = PlayerMovement.instance.transform.position + PlayerMovement.instance.transform.forward * beamMeetDistance;
    }

    protected override void OnStart()
    {
        creature.GetComponent<LineRenderer>().enabled = true;
        
        CreatureBehavior idle = new WispBehaviorIdle((Wisp)creature);
        creature.AddBehavior(idle);
        dependants.Add(idle);

        creature.GetComponent<LineRenderer>().SetPosition(0, creature.transform.position);
        creature.GetComponent<LineRenderer>().SetPosition(1, PlayerMovement.instance.transform.position);
    }

    protected override void OnFixedUpdate()
    {
        if (elapsed > duration)
        {
            creature.GetComponent<LineRenderer>().enabled = false;
            creature.AddBehaviorToDeletionQueue(this);
            return;
        }

        creature.GetComponent<LineRenderer>().SetPosition(1, Vector3.Lerp(creature.transform.position, PlayerMovement.instance.transform.position, (float) elapsed / duration));
    }
}