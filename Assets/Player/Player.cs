using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance { get; private set; }

    [SerializeField] private float health;

    [HideInInspector] public bool weaponsEnabled = true;

    private void Start()
    {
        instance = this;
    }

    private void FixedUpdate()
    {
        
    }

    public void Damage(float damage)
    {
        health -= damage;
    }
}