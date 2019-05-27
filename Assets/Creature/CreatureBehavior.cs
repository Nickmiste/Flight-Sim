using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CreatureBehavior
{
    protected Creature creature;
    protected List<CreatureBehavior> dependants = new List<CreatureBehavior>();

    private BehaviorPhase[] phases;
    private int currentPhaseIndex = 0;
    protected bool loop = false;

    protected int elapsed { get; private set; }
    protected int elapsedPhase { get; private set; }

    public CreatureBehavior(Creature creature)
    {
        this.creature = creature;
        this.elapsed = -1;
        this.elapsedPhase = -1;
    }

    public void Start()
    {
        OnStart();
        phases = GetPhases();
        if (phases.Length >= 0)
            phases[0].Start();
    }

    public void FixedUpdate()
    {
        elapsed++;
        elapsedPhase++;
        OnFixedUpdate();
        
        if (ShouldMoveToNextPhase())
        {
            elapsedPhase = 0;
            if (++currentPhaseIndex == phases.Length)
            {
                if (loop)
                    currentPhaseIndex = 0;
                else
                {
                    creature.AddBehaviorToDeletionQueue(this);
                    return;
                }
            }

            phases[currentPhaseIndex].Start();
        }

        phases[currentPhaseIndex].FixedUpdate();
    }

    private bool ShouldMoveToNextPhase()
    {
        if (phases[currentPhaseIndex].forceNextPhase)
            return true;

        if (phases[currentPhaseIndex].length == -1)
            return false;
        else
            return elapsedPhase >= phases[currentPhaseIndex].length;
    }

    protected virtual void OnStart() { }
    protected virtual void OnFixedUpdate() { }
    protected virtual void OnDelete() { }

    public void SetDeleted()
    {
        OnDelete();
    }

    public List<CreatureBehavior> GetDependants()
    {
        return dependants;
    }

    protected virtual BehaviorPhase[] GetPhases()
    {
        return new BehaviorPhase[] { new DefaultBehaviorPhase(this) };
    }

    private class DefaultBehaviorPhase : BehaviorPhase
    {
        public DefaultBehaviorPhase(CreatureBehavior behavior, int length = -1) : base(behavior, length) { }
    }

    protected abstract class BehaviorPhase
    {
        protected CreatureBehavior behavior { get; private set; }
        public int length { get; private set; }
        public bool forceNextPhase { get; private set; }

        protected BehaviorPhase(CreatureBehavior behavior, int length)
        {
            this.behavior = behavior;
            this.length = length;
        }

        public virtual void Start() { }
        public virtual void FixedUpdate() { }

        protected void ForceNextPhase()
        {
            this.forceNextPhase = true;
        }

        protected int GetElapsedPhase()
        {
            return behavior.elapsedPhase;
        }

        protected float GetPhaseProgress()
        {
            return (float)behavior.elapsedPhase / length;
        }

        protected Creature GetCreature()
        {
            return behavior.creature;
        }
    }
}