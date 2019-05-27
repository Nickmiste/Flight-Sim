using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    int num = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
            Debug.Log(num++);
    }
}