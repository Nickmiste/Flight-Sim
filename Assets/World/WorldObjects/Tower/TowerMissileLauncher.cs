using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMissileLauncher : MonoBehaviour
{
    //Initialized in Tower
    [HideInInspector] public GameObject towerProjectile;
    [HideInInspector] public int numShots;
    [HideInInspector] public float damage;
    [HideInInspector] public int attackTime;
    [HideInInspector] public int cooldown;
    [HideInInspector] public float maxRevolutionRadius;
    [HideInInspector] public Vector2 control1;
    [HideInInspector] public Vector2 control2;

    private int cooldownRemaining = 0;
    private bool attackInProgress = false;
    
    private LinkedList<GameObject> targetsInRange = new LinkedList<GameObject>();

    private void FixedUpdate()
    {
        if (!attackInProgress && cooldownRemaining > 0)
            cooldownRemaining--;

        if (cooldownRemaining == 0)
        {
            GameObject target = targetsInRange.First?.Value;
            while (target == null)
            {
                if (targetsInRange.Count > 1)
                {
                    targetsInRange.RemoveFirst();
                    target = targetsInRange.First.Value;
                }
                else
                    return;
            }
            
            StartCoroutine(Fire(target));
            cooldownRemaining = cooldown;
            attackInProgress = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
            targetsInRange.AddFirst(other.gameObject);
        else if (other.GetComponent<IDamageable>() != null)
            targetsInRange.AddLast(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        targetsInRange.Remove(other.gameObject);
    }

    private IEnumerator Fire(GameObject target)
    {
        GameObject[] shots = new GameObject[numShots];
        for (int i = 0; i < numShots; i++)
        {
            GameObject shot = Instantiate(towerProjectile, transform);
            shot.GetComponent<TowerMissile>().damage = damage;
            shots[i] = shot;
        }

        Vector3 arcDirection = Vector3.ProjectOnPlane(Random.onUnitSphere, transform.up);
        Vector3 curvePosition = Vector3.zero;
        Vector3 lastCurvePosition;

        for (int i = 0; i < attackTime; i++)
        {
            if (target == null)
            {
                foreach (GameObject shot in shots)
                    Destroy(shot);
                break;
            }

            float progress = (float)i / attackTime;
            Vector3 controlStart;
            Vector3 controlEnd;
            {
                float distance = Vector3.Distance(transform.position, target.transform.position);

                controlStart = Vector3.Lerp(transform.position, target.transform.position, control1.x);
                controlStart += arcDirection * distance * control1.y;
                controlEnd = Vector3.Lerp(transform.position, target.transform.position, control2.x);
                controlEnd += arcDirection * distance * control2.y;
            }
            lastCurvePosition = curvePosition;
            curvePosition = VectorUtil.BezierInterpolate(transform.position, target.transform.position,
                                                         controlStart, controlEnd, progress);
            Vector3 forward = (curvePosition - lastCurvePosition).normalized;
            Vector3 baseRot = Vector3.ProjectOnPlane(new Vector3(1, 0, 0), forward).normalized;

            for (int j = 0; j < shots.Length; j++)
            {
                if (shots[j] == null)
                    continue;

                float theta = 360 * ((float)j / numShots);
                float distance = (maxRevolutionRadius/2) * Mathf.Sin(2*Mathf.PI * progress - (Mathf.PI/2)) + (maxRevolutionRadius/2);
                distance = Mathf.Pow(Mathf.Sin(Mathf.PI * progress), 2) * maxRevolutionRadius;

                Vector3 offset = Quaternion.AngleAxis(theta, forward) * baseRot;
                offset *= distance;

                shots[j].transform.SetPositionAndRotation(curvePosition + offset,
                                                          Quaternion.LookRotation(forward, offset));
            }
            yield return null;
        }

        attackInProgress = false;
    }
}