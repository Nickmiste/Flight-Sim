using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidField : MonoBehaviour
{
    [SerializeField] private GameObject asteroid = null;

    public const int MAX_ASTEROIDS = 100;

    public void FixedUpdate()
    {
        foreach (Transform child in transform)
            if (!PlayerUtil.IsInViewDistance(child.position))
                Destroy(child.gameObject);

        while (transform.childCount < MAX_ASTEROIDS)
            SpawnAsteroid(PlayerUtil.GetRandomPointWithinRange(200, 1000));

        //Vector3 closest = transform.position;

        //for (float i = 0; i < 100; i += 1 / density)
        //{
        //    if (transform.childCount < MAX_ASTEROIDS)
        //        SpawnAsteroid(closest + (transform.forward * i) + (Random.insideUnitSphere * radius));
        //    else break;

        //    if (transform.childCount < MAX_ASTEROIDS)
        //        SpawnAsteroid(closest - (transform.forward * i) + (Random.insideUnitSphere * radius));
        //    else break;
        //}
    }

    private void SpawnAsteroid(Vector3 position)
    {
        GameObject obj = Instantiate(asteroid) as GameObject;
        obj.transform.parent = transform;
        obj.transform.position = position;
        obj.transform.rotation = Random.rotation;
        float velocityRandom = Random.value + 0.5f;
        obj.GetComponent<Rigidbody>().AddForce(Random.onUnitSphere * 1000 * velocityRandom);
        obj.GetComponent<Rigidbody>().AddTorque(Random.insideUnitSphere * 100);
    }
}
