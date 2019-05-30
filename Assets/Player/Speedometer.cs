using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    private static Speedometer instance;

    [SerializeField] private ProgressBar throttleBar = null;
    [SerializeField] private ProgressBar speedBar = null;
    [SerializeField] private ProgressBar forwardSpeedBar = null;

    private void Start()
    {
        instance = this;
    }

    public static void UpdateSpeedometerUI(float throttle, float speed, float forwardSpeed)
    {
        instance.throttleBar.SetProgress(throttle);
        instance.speedBar.SetProgress(speed);
        instance.forwardSpeedBar.SetProgress(forwardSpeed);
    }
}