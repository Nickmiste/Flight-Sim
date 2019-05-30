using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour, ITargetable
{
    [SerializeField] private GameObject towerProjectile = null;
    [SerializeField] private int numShots = 0;
    [SerializeField] private float damage = 0;
    [SerializeField] private int attackTime = 0;
    [SerializeField] private int cooldown = 0;
    [SerializeField] private float maxRevolutionRadius = 0;
    [Header("Bezier Curve Settings")]
    [SerializeField] private Vector2 control1 = Vector2.zero;
    [SerializeField] private Vector2 control2 = Vector2.zero;


    private void Start()
    {
        foreach (TowerMissileLauncher launcher in GetComponentsInChildren<TowerMissileLauncher>())
        {
            launcher.towerProjectile = towerProjectile;
            launcher.numShots = numShots;
            launcher.attackTime = attackTime;
            launcher.cooldown = cooldown;
            launcher.damage = damage;
            launcher.maxRevolutionRadius = maxRevolutionRadius;
            launcher.control1 = control1;
            launcher.control2 = control2;
        }
    }

    string ITargetable.GetTargetInfo()
    {
        return "An enemy tower.\nVery dangerous!";
    }
}
