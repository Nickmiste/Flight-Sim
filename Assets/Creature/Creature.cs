using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Creature : MonoBehaviour, IDamageable
{
    public float health { get; private set; }

    public Vector3 origin { get; private set; }
    public int ticksAlive { get; private set; }
    public int ticksIdle { get; private set; }

    public bool invulnerable { get; set; }
    public bool pierceOverride { get; set; }

    private List<CreatureBehavior> behaviors = new List<CreatureBehavior>();
    private List<CreatureBehavior> deletionQueue = new List<CreatureBehavior>();
    protected CreatureBehavior idleBehavior = null;
    protected DeathBehavior deathBehavior = null;

    private void Start()
    {
        health = GetMaxHealth();
        origin = transform.position;
        ticksAlive = -1;
        ticksIdle = -1;

        OnStart();

        deathBehavior = deathBehavior ?? new DefaultDeathBehavior(this);
    }
    
    private void FixedUpdate()
    {
        if (IsDead())
        {
            deathBehavior.FixedUpdate();
            return;
        }

        ticksAlive++;
        ticksIdle = IsIdle() ? ticksIdle + 1 : -1;

        OnFixedUpdate();

        if (idleBehavior != null)
        {
            if (behaviors.Count == 0)
                AddBehavior(idleBehavior);
            else if (behaviors.Count > 1)
                RemoveBehaviorInstantly(idleBehavior);
        }

        foreach (CreatureBehavior behavior in behaviors)
            behavior.FixedUpdate();
        foreach (CreatureBehavior behavior in deletionQueue)
            behaviors.Remove(behavior);
        deletionQueue.Clear();
    }

    protected abstract float GetMaxHealth();
    protected virtual void OnStart() { }
    protected virtual void OnFixedUpdate() { }

    void IDamageable.Damage(float damage)
    {
        health -= damage;

        if (IsDead())
            deathBehavior.Start();
    }

    public void Kill()
    {
        health = 0;
        if (IsDead())
            deathBehavior.Start();
    }

    public bool IsDead()
    {
        return health <= 0;
    }

    protected bool IsIdle()
    {
        return behaviors.Count == 0 || behaviors.Contains(idleBehavior);
    }

    public void AddBehavior(CreatureBehavior behavior)
    {
        behaviors.Add(behavior);
        behavior.Start();
    }

    public void AddBehaviorToDeletionQueue(CreatureBehavior behavior)
    {
        behavior.SetDeleted();
        deletionQueue.Add(behavior);
        foreach (CreatureBehavior dependant in behavior.GetDependants())
            AddBehaviorToDeletionQueue(dependant);
    }

    public void RemoveBehaviorInstantly(CreatureBehavior behavior)
    {
        behavior.SetDeleted();
        behaviors.Remove(behavior);
        foreach (CreatureBehavior dependant in behavior.GetDependants())
            RemoveBehaviorInstantly(dependant);
    }

    public bool ContainsBehavior<T>()
    {
        foreach (CreatureBehavior behavior in behaviors)
            if (behavior is T)
                    return true;
        return false;
    }
}