using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourceManager
{
    private static Dictionary<string, string> resourcePaths = new Dictionary<string, string>();

    static ResourceManager()
    {
        resourcePaths.Add("turret", "Turret");
        resourcePaths.Add("beam", "Beam");
        resourcePaths.Add("gate", "World/Worldgate");
    }

    public static Object Load(string resource)
    {
        Object obj = Resources.Load(resourcePaths[resource.ToLower()]);
        if (obj == null)
            Debug.LogError("Failed to load resource: " + resource);
        return obj;
    }
}