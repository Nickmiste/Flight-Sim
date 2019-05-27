using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nebula : MonoBehaviour, IInitializable
{
    private float radius;
    private float radiusSquared;
    private float Radius
    {
        get { return radius; }
        set
        {
            transform.localScale = new Vector3(value * 2, value * 2, value * 2);
            radius = value;
            radiusSquared = value * value;
        }
    }

    public void Init(object[] data)
    {
        Radius = (float) data[0];
    }

    void FixedUpdate()
    {
        if (VectorUtil.SqrDistance(transform.position, PlayerMovement.instance.transform.position) > radiusSquared)
        {
            //Damage Player
        }
    }
}
