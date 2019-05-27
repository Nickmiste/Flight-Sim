using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muzzle : MonoBehaviour
{
    private float flashIntensity;

    private const int NUM_FRAMES = 10;
    private int framesLeft = 0;

    private void Start()
    {
        flashIntensity = GetComponent<Light>().intensity;
        EndAnimation();
    }

    public void Fire()
    {
        framesLeft = NUM_FRAMES;

        GetComponent<Light>().intensity = flashIntensity;
        GetComponent<Light>().enabled = true;
    }

    private void EndAnimation()
    {
        GetComponent<Light>().enabled = false;
    }

    private void Update()
    {
        if (framesLeft > 0)
            framesLeft--;
        else
        {
            EndAnimation();
            return;
        }

        GetComponent<Light>().intensity = Mathf.Lerp(0, flashIntensity, ((float)framesLeft) / NUM_FRAMES);
    }
}