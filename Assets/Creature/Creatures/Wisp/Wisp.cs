using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wisp : Creature
{
    public static List<Wisp> wisps = new List<Wisp>();
    private const int spawnCount = 250; //number of wisps that spawn when a new wisp cloud is formed
    private const int minWisps = 25; //once wisp count gets below this value, remaining wisps die
    
    [SerializeField] private float aggroRange = 0;
    [SerializeField] private float targetRange = 0;
    [SerializeField] private float beamMeetDistance = 0;
    [SerializeField] public ParticleSystem disappearParticles;

    private Vector3 baseScale;
    public Vector3 BaseScale { get { return baseScale; } }

    protected override float GetMaxHealth()
    {
        return 1;
    }

    protected override void OnStart()
    {
        if (wisps.Count == 0)
        {
            for (int i = 0; i < spawnCount; i++)
                wisps.Add(Instantiate(this));
            Destroy(gameObject);
            return;
        }
        
        pierceOverride = true;
        baseScale = Vector3.Lerp(transform.localScale, transform.localScale / 2, Random.value);
        idleBehavior = new WispBehaviorIdle(this);
        deathBehavior = new WispDeathBehavior(this);
    }

    protected override void OnFixedUpdate()
    {
        if (wisps.Count < minWisps)
        {
            ((WispDeathBehavior)deathBehavior).finalDeath = true;
            Kill();
            return;
        }

        if (ticksIdle > 100)
            if (VectorUtil.SqrDistance(transform.position, PlayerMovement.instance.transform.position) <= aggroRange * aggroRange)
            {
                if (random(ticksAlive) < 0.5f)
                    AddBehavior(new WispBehaviorFire(this, beamMeetDistance));
                else
                    AddBehavior(new WispBehaviorMove(this, targetRange));
            }
    }

    private float random(float x)
    {
        float num = Mathf.Sin(x) * 100000;
        return num - Mathf.Floor(num);
    }

    //public static int getNumWispsInHiveMind(int id)
    //{
    //    int num = 0;
    //    foreach (Wisp wisp in wisps.Keys)
    //        if (wisps[wisp] == id)
    //            num++;
    //    return num;
    //}


    //private static void RemoveHiveMind(int id)
    //{
    //    print(getNumWispsInHiveMind(0));
    //    foreach (Wisp wisp in wisps.Keys)
    //    {
    //        ((WispDeathBehavior)wisp.deathBehavior).finalDeath = true;
    //        wisp.Kill();
    //    }
    //    print(getNumWispsInHiveMind(0));
    //}
}