using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MiscUtil
{
    public static bool HasAttribue<T>(GameObject obj)
    {
        foreach (MonoBehaviour script in obj.GetComponents<MonoBehaviour>())
            foreach (System.Attribute testAttribute in System.Attribute.GetCustomAttributes(script.GetType()))
                if (testAttribute is T)
                    return true;
        return false;
    }
}
