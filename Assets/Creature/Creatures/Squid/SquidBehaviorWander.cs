using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquidBehaviorWander : CreatureBehavior
{
    public SquidBehaviorWander(Creature creature) : base(creature)
    {
        loop = true;
    }

    protected override BehaviorPhase[] GetPhases()
    {
        return new BehaviorPhase[] { new TurnPhase(this), new MovePhase(this) };
    }

    private class TurnPhase : BehaviorPhase
    {
        private Quaternion target;

        public TurnPhase(CreatureBehavior behavior) : base(behavior, 100) { }

        public override void Start()
        {
            Debug.Log(true);
            Vector3 lookTarget = GetCreature().transform.position + Random.onUnitSphere;
            target = Quaternion.LookRotation(lookTarget - GetCreature().transform.position);
        }

        public override void FixedUpdate()
        {
            GetCreature().transform.rotation = Quaternion.Lerp(GetCreature().transform.rotation, target, GetPhaseProgress());
        }
    }

    private class MovePhase : BehaviorPhase
    {
        private Vector3 start;
        private Vector3 target;

        public MovePhase(CreatureBehavior behavior) : base(behavior, 30) { }

        public override void Start()
        {
            start = GetCreature().transform.position;
            target = start + GetCreature().transform.up * 5;
        }

        public override void FixedUpdate()
        {
            //GetCreature().transform.position = Vector3.Lerp(start, target, GetPhaseProgress());

            GetCreature().transform.position = VectorUtil.InterpolateCustom(start, target, (T) => Mathf.Pow(T, 0.3f), GetPhaseProgress());
        }

        private float foo(float x)
        {
            return 1;
        }
    }
}
