using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretData
{
    public enum Effect { BURN, EXPLODE, BREACH }
    public enum FirePath { SIN_VERT, COS_HOR, GUIDED, BLUB }

    public float damage = 1;
    public float speed = 6;
    public int cooldown = 15;
    public float size = 1;
    public bool pierce = false;
    public List<Effect> effects = new List<Effect>();
    public List<FirePath> firePaths = new List<FirePath>();

    public TurretData()
    {

    }
}
