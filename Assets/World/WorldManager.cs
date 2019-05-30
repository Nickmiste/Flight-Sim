using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class WorldManager : MonoBehaviour
{
    public static WorldManager instance { get; private set; }

    public float dangerLevel { get; private set; }
    public float radius { get; private set; }

    [SerializeField] private GameObject[] worldObjectPrefabs = null;

    private void Start()
    {
        instance = this;
        LoadWorld(0);
    }

    public void LoadWorld(float dangerLevel)
    {
        this.dangerLevel = dangerLevel;
        this.radius = 10000;

        PlayerMovement.instance?.Reset();

        foreach (Transform child in transform)
            GameObject.Destroy(child.gameObject);
        transform.DetachChildren();

        //GenerateNebula();
        GenerateWorldGates();
        //GenerateStations();

        GenerateAsteroidField();
        GenerateTowers();

        Debug.Log(string.Format("Finished loading world with {0} world objects.", transform.childCount));
    }

    private static GameObject CreateWorldObject<T>(float distanceFromOrigin = 0, object[] data = null) => CreateWorldObject<T>(Random.onUnitSphere * distanceFromOrigin, Random.rotation, data);
    private static GameObject CreateWorldObject<T>(Vector3 position) => CreateWorldObject<T>(position, Random.rotation, null);
    private static GameObject CreateWorldObject<T>(Vector3 position, object[] data) => CreateWorldObject<T>(position, Random.rotation, data);
    private static GameObject CreateWorldObject<T>(Vector3 position, Quaternion rotation) => CreateWorldObject<T>(position, rotation, null);
    private static GameObject CreateWorldObject<T>(Vector3 position, Quaternion rotation, object[] data)
    {
        GameObject obj;
        try
        {
            obj = GameObject.Instantiate(GetGameObject<T>()) as GameObject;
        }
        catch (System.ArgumentException)
        {
            Debug.LogError(string.Format("Tried to create a world object of type '{0}' but it is missing from the prefab list.", typeof(T)));
            return null;
        }

        obj.transform.parent = WorldManager.instance.transform;
        obj.transform.position = position;
        obj.transform.rotation = rotation;

        try
        {
            obj.GetComponent<IInitializable>()?.Init(data);
        }
        catch (System.Exception e)
        {
            Debug.LogError(string.Format("Error initializing world object of type '{0}' at {1}.\n" + e, typeof(T), position));
            Destroy(obj);
            return null;
        }
        
        Debug.Log(string.Format("Generated world object of type '{0}'.", typeof(T)));
        return obj;
    }

    private static GameObject GetGameObject<T>()
    {
        foreach (GameObject obj in WorldManager.instance.worldObjectPrefabs)
            if (obj.GetComponent<T>() != null)
                return obj;
        return null;
    }

    /*****************************************************************************************************************************************************************
    Generation Methods
    ******************************************************************************************************************************************************************/

    private void GenerateNebula()
    {
        CreateWorldObject<Nebula>(0, new object[] { radius });
    }

    private void GenerateWorldGates()
    {
        int numGates = 3;

        for (int i = 0; i < numGates; i++)
        {
            float distance = 500;
            float newDangerLevel = 3;

            CreateWorldObject<WorldGate>(distance, new object[] { newDangerLevel });
        }
    }

    private void GenerateStations()
    {
        int numStations = 3;

        for (int i = 0; i < numStations; i++)
        {
            float distance = 500;

            CreateWorldObject<Station>(distance);
        }
    }

    private void GenerateAsteroidField()
    {
        CreateWorldObject<AsteroidField>();
    }

    private void GenerateTowers()
    {
        CreateWorldObject<Tower>(500);
    }
}