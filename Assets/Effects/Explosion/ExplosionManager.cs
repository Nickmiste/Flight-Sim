using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    public static ExplosionManager instance { get; private set; }

    [SerializeField] private GameObject explosion = null;

    private void Start()
    {
        instance = this;
    }

    public static void CreateExplosion(Vector3 position)
    {
        Instantiate(instance.explosion, position, Quaternion.identity);
    }
}