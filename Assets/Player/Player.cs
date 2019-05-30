using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public static Player instance { get; private set; }

    [SerializeField] private ProgressBar hullBar = null;
    [SerializeField] private float maxHull = 100f;
    private float hull;


    [HideInInspector] public bool weaponsEnabled = true;
    
    private void Start()
    {
        instance = this;
        hull = maxHull;
    }

    private void FixedUpdate()
    {
        
    }

    void IDamageable.Damage(float damage)
    {
        hull -= damage;
        hullBar.SetProgress(hull / maxHull);
    }
}