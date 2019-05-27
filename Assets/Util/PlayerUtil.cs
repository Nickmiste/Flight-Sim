using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerUtil
{
    public static Vector3 GetPosition() => Player.instance.transform.position;
    public static float GetViewDistance() => Player.instance.GetComponentInChildren<Camera>().farClipPlane;

    public static bool IsInViewDistance(Vector3 pos)
    {
        return VectorUtil.SqrDistance(GetPosition(), pos) <= GetViewDistance() * GetViewDistance();
    }

    public static Vector3 GetRandomPointInViewDistance()
    {
        return GetPosition() + Random.insideUnitSphere * GetViewDistance();
    }

    public static Vector3 GetRandomPointWithinRange(float min, float max)
    {
        Vector3 random = Random.onUnitSphere;
        return GetPosition() + Vector3.Lerp(random * min, random * max, Random.value);
    }
}
