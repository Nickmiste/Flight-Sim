using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamParticles : MonoBehaviour
{
    public void DestroyParticleSystem()
    {
        transform.parent = null;
#pragma warning disable CS0618 // Type or member is obsolete
        GetComponent<ParticleSystem>().emissionRate = 0;
#pragma warning restore CS0618 // Type or member is obsolete
        Destroy(gameObject, 5);
    }
}