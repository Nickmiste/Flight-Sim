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

    public static Vector3 BezierInterpolate(Vector3 start, Vector3 end, Vector3 controlStart, Vector3 controlEnd, float t)
    {
        Debug.DrawLine(start, controlStart, Color.blue);
        Debug.DrawLine(controlStart, controlEnd, Color.blue);
        Debug.DrawLine(controlEnd, end, Color.blue);

        Vector3 p1 = Vector3.Lerp(start, controlStart, t);
        Vector3 p2 = Vector3.Lerp(controlStart, controlEnd, t);
        Vector3 p3 = Vector3.Lerp(controlEnd, end, t);

        Debug.DrawLine(p1, p2, Color.red);
        Debug.DrawLine(p2, p3, Color.red);

        Vector3 p4 = Vector3.Lerp(p1, p2, t);
        Vector3 p5 = Vector3.Lerp(p2, p3, t);

        Debug.DrawLine(p4, p5, Color.green);
        
        return Vector3.Lerp(p4, p5, t);
    }
}
