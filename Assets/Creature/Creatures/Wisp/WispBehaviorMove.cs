using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WispBehaviorMove : CreatureBehavior
{
    private const float fadeOutSize = 0.3f;
    private const float overshootSizeMod = 2;

    private readonly Vector3 startPosition;
    private readonly Vector3 targetPosition;

    public WispBehaviorMove(Wisp wisp, float targetRange) : base(wisp)
    {
        startPosition = wisp.transform.position;

        targetPosition = PlayerMovement.instance.transform.position + PlayerMovement.instance.transform.forward * targetRange;
        targetPosition += Random.insideUnitSphere * (targetRange / 15);
    }

    protected override void OnStart()
    {
        creature.transform.localScale = ((Wisp)creature).BaseScale;
    }

    protected override void OnDelete()
    {
        creature.transform.localScale = ((Wisp)creature).BaseScale;
    }

    protected override BehaviorPhase[] GetPhases()
    {
        return new BehaviorPhase[] { new FadeOutPhase(this), new RepositionPhase(this), new FadeInPhase(this) };
    }

    private class FadeOutPhase : BehaviorPhase
    {
        public FadeOutPhase(CreatureBehavior behavior) : base(behavior, 15) { }

        public override void FixedUpdate()
        {
            GetCreature().transform.localScale = VectorUtil.Lerp3(((Wisp)GetCreature()).BaseScale, ((Wisp)GetCreature()).BaseScale * overshootSizeMod, Vector3.one * fadeOutSize, 0.7f, GetPhaseProgress());
        }
    }

    private class RepositionPhase : BehaviorPhase
    {
        public RepositionPhase(CreatureBehavior behavior) : base(behavior, 75) { }

        public override void Start()
        {
            GetCreature().transform.localScale = Vector3.one * fadeOutSize;
            ((Wisp)GetCreature()).disappearParticles.Play();
            ((Wisp)GetCreature()).invulnerable = true;
        }

        public override void FixedUpdate()
        {
            GetCreature().transform.position = Vector3.Lerp(((WispBehaviorMove)behavior).startPosition, ((WispBehaviorMove)behavior).targetPosition, GetPhaseProgress());
        }
    }

    private class FadeInPhase : BehaviorPhase
    {
        public FadeInPhase(CreatureBehavior behavior) : base(behavior, 30) { }

        public override void Start()
        {
            ((Wisp)GetCreature()).invulnerable = false;
        }

        public override void FixedUpdate()
        {
            GetCreature().transform.localScale = VectorUtil.Lerp3(Vector3.one * fadeOutSize, ((Wisp)GetCreature()).BaseScale * overshootSizeMod, ((Wisp)GetCreature()).BaseScale, 0.3f, GetPhaseProgress());
        }
    }
}