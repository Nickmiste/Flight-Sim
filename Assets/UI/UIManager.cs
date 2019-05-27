using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    private static int steps = 100;

    private void Start()
    {
        instance = this;
    }

    public static void SetSpeedUI(float throttle, float speed, float forwardSpeed)
    {
        //lower resolution of bars
        throttle = Mathf.Floor(throttle * steps) / steps;
        speed = Mathf.Floor(speed * steps) / steps;
        forwardSpeed = Mathf.Floor(forwardSpeed * steps) / steps;

        instance.transform.Find("Throttle/Throttle").GetComponent<Image>().fillAmount = throttle;
        instance.transform.Find("Speedometer/Speed").GetComponent<Image>().fillAmount = speed;
        instance.transform.Find("Speedometer/ForwardSpeed").GetComponent<Image>().fillAmount = forwardSpeed;
    }
}
