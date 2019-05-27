//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class EnemyBehaviorFaceTarget : EnemyBehavior
//{
//    private Vector3 target;

//    public EnemyBehaviorFaceTarget(Enemy enemy, Vector3 target) : base(enemy)
//    {
//        this.target = target;
//    }

//    public override void Start()
//    {

//    }

//    public override void FixedUpdate()
//    {
//        //float deltaPitch = Mathf.Acos((transform.position.y - target.y) / Vector3.Distance(transform.position, target));
//        //Vector3 deltaPosition = target - transform.position;
//        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(deltaPosition), 1);
//        //Vector3 targetEuler = Quaternion.LookRotation(target - transform.position).eulerAngles;
//        //Vector3 targetForward = (target - transform.position).normalized;

//        float targetPitchTheta = Mathf.Acos((target.y - enemy.transform.position.y) / Vector3.Distance(enemy.transform.position, target));
//        float currentPitchTheta = Mathf.Acos(enemy.transform.forward.y);
//        float targetYawTheta = Mathf.Acos((target.z - enemy.transform.position.z) / Vector3.Distance(enemy.transform.position, target));
//        float currentYawTheta = Mathf.Acos(enemy.transform.forward.z);

//        //if (target.z - enemy.transform.position.z < 0)
//        //    enemy.pitch = Mathf.Sign(currentPitchTheta - targetPitchTheta);
//        //else
//        //    enemy.pitch = Mathf.Sign(targetPitchTheta - currentPitchTheta);

//        if (target.x - enemy.transform.position.x < 0)
//            enemy.yaw = Mathf.Sign(currentYawTheta - targetYawTheta);
//        else
//            enemy.yaw = Mathf.Sign(targetYawTheta - currentYawTheta);

//    }

//    public void SetTarget(Vector3 target)
//    {
//        this.target = target;
//    }
//}
