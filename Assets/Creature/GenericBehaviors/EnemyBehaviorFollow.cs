//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class EnemyBehaviorFollow : EnemyBehavior
//{
//    private GameObject target;
//    private float followDistance;
//    private EnemyBehaviorFaceTarget faceTarget;

//    public EnemyBehaviorFollow(Enemy enemy, GameObject target, float followDistance) : base(enemy)
//    {
//        this.target = target;
//        this.followDistance = followDistance;
//    }

//    public override void Start()
//    {
//        enemy.throttle = 0; //1
//        faceTarget = new EnemyBehaviorFaceTarget(enemy, target.transform.position);
//        enemy.AddBehavior(faceTarget);
//        dependants.Add(faceTarget);
//    }

//    public override void FixedUpdate()
//    {
//        faceTarget.SetTarget(target.transform.position);
//        if (Vector3.Distance(enemy.transform.position, target.transform.position) < followDistance)
//            enemy.throttle = 0;
//        else
//            enemy.throttle = 0; //1
//    }
//}
