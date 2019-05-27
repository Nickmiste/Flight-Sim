//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class EnemyBehaviorReposition : EnemyBehavior
//{
//    private Vector3 target;
//    private EnemyBehaviorFaceTarget faceTarget;

//    public EnemyBehaviorReposition(Enemy enemy, Vector3 target) : base(enemy)
//    {
//        this.target = target;
//    }

//    public override void Start()
//    {
//        enemy.throttle = 1;
//        faceTarget = new EnemyBehaviorFaceTarget(enemy, target);
//        enemy.AddBehavior(faceTarget);
//        dependants.Add(faceTarget);
//    }

//    public override void FixedUpdate()
//    {
//        if (Vector3.Distance(enemy.transform.position, target) < 1)
//        {
//            enemy.throttle = 0;
//            enemy.AddBehaviorToDeletionQueue(this);
//        }
//    }
//}
