using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorUtil
{
    public static float SqrDistance(Vector3 a, Vector3 b)
    {
        return (a - b).sqrMagnitude;
    }

    public static Vector3 Lerp3(Vector3 a, Vector3 b, Vector3 c, float bt, float t)
    {
        if (t < bt)
            return Vector3.Lerp(a, b, t / bt);
        else
            return Vector3.Lerp(b, c, (t - bt) / (1 - bt));
    }

    public static Vector3 InterpolateCustom(Vector3 a, Vector3 b, Func<float, float> expression, float t)
    {
        float newT = expression(t);
        return Vector3.Lerp(a, b, newT);
    }
}
