using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public abstract class WorldObject
//{
//    public GameObject gameObject;

//    public WorldObject(GameObject gameObject)
//    {
//        this.gameObject = gameObject;
//    }
//}

//public static class WorldObject
//{
    //public static GameObject CreateWorldObject<T>(Vector3 position, byte data) => CreateWorldObject<T>(position, Random.rotation);
    //public static GameObject CreateWorldObject<T>(Vector3 position, Quaternion rotation, byte data)
    //{
    //    GameObject obj = CreateWorldObject<T>(position, rotation);
    //    Initializer.Init(obj, data);
    //    return obj;
    //}

    //public static GameObject CreateWorldObject<T>(Vector3 position, ushort data) => CreateWorldObject<T>(position, Random.rotation);
    //public static GameObject CreateWorldObject<T>(Vector3 position, Quaternion rotation, ushort data)
    //{
    //    GameObject obj = CreateWorldObject<T>(position, rotation);
    //    Initializer.Init(obj, data);
    //    return obj;
    //}

    //public static GameObject CreateWorldObject<T>(Vector3 position, uint data) => CreateWorldObject<T>(position, Random.rotation);
    //public static GameObject CreateWorldObject<T>(Vector3 position, Quaternion rotation, uint data)
    //{
    //    GameObject obj = CreateWorldObject<T>(position, rotation);
    //    Initializer.Init(obj, data);
    //    return obj;
    //}

    //public static GameObject CreateWorldObject<T>(Vector3 position, ulong data) => CreateWorldObject<T>(position, Random.rotation);
    //public static GameObject CreateWorldObject<T>(Vector3 position, Quaternion rotation, ulong data)
    //{
    //    GameObject obj = CreateWorldObject<T>(position, rotation);
    //    Initializer.Init(obj, data);
    //    return obj;
    //}

    //public static GameObject CreateWorldObject<T>(Vector3 position) => CreateWorldObject<T>(position, Random.rotation);
    //public static GameObject CreateWorldObject<T>(Vector3 position, Quaternion rotation)
    //{
    //    GameObject obj = GameObject.Instantiate(GetGameObject<T>()) as GameObject;
    //    if (obj == null)
    //        return null;

    //    obj.transform.parent = WorldManager.instance.transform;
    //    obj.transform.position = position;
    //    obj.transform.rotation = rotation;
    //    return obj;
    //}

    //private static GameObject GetGameObject<T>()
    //{
    //    foreach (GameObject obj in WorldManager.instance.worldObjectPrefabs)
    //        if (obj is T)
    //            return obj;
    //    Debug.Log(string.Format("Tried to create a world object of type '{0}' but it is missing from the prefab list.", typeof(T)));
    //    return null;
    //}
//}