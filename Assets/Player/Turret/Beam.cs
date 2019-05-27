using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    private static Type[] typesToIgnore = new Type[] { typeof(Player), typeof(Beam) };

    public TurretData data;
    private Vector3 dir;
    private int elapsed = -1;

    void Start()
    {
        dir = (TargetManager.target - this.transform.position).normalized;
        transform.localScale = new Vector3(data.size, data.size, data.size);
        Destroy(gameObject, 5f);
        
        for (int i = 0; i < GetComponent<LineRenderer>().positionCount; i++)
            GetComponent<LineRenderer>().SetPosition(i, transform.position);
    }

    void FixedUpdate()
    {
        elapsed++;
        HandleFirePath();

        for (int i = GetComponent<LineRenderer>().positionCount - 1; i > 0; i--)
            GetComponent<LineRenderer>().SetPosition(i, GetComponent<LineRenderer>().GetPosition(i - 1));
        GetComponent<LineRenderer>().SetPosition(0, transform.position);
    }

    void HandleFirePath()
    {
        this.transform.position += dir * data.speed;

        if (data.firePaths.Contains(TurretData.FirePath.SIN_VERT))
            this.transform.position += transform.up * Mathf.Sin(elapsed / 2f);
        if (data.firePaths.Contains(TurretData.FirePath.COS_HOR))
            this.transform.position += transform.right * Mathf.Cos(elapsed / 2f);
        if (data.firePaths.Contains(TurretData.FirePath.GUIDED))
            dir = (TargetManager.target - this.transform.position).normalized;
        if (data.firePaths.Contains(TurretData.FirePath.BLUB))
            this.transform.localScale += Vector3.one.normalized * Mathf.Sin(elapsed / 3f) * 0.3f;
    }

    void OnDestroy()
    {
        GetComponentInChildren<BeamParticles>()?.DestroyParticleSystem();
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (Type type in typesToIgnore)
            if (other.gameObject.GetComponent(type) != null)
                return;

        bool pierceOverride = false;
        if (other.gameObject.GetComponent<Creature>() != null)
        {
            if (other.gameObject.GetComponent<Creature>().invulnerable)
                return;
            pierceOverride = other.gameObject.GetComponent<Creature>().pierceOverride;
        }

        other.gameObject.GetComponent<IDamageable>()?.Damage(data.damage);
        
        if (!data.pierce && !pierceOverride)
            Destroy(gameObject);
    }
}