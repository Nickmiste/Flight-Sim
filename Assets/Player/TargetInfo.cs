using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetInfo : MonoBehaviour
{
    private const int ANIMATION_TIME = 20;

    private enum State { OPENED, CLOSED, OPENING, CLOSING }
    private State state = State.CLOSING;
    private float status = 0;

    [SerializeField] private Text title = null;
    [SerializeField] private Text info = null;
    [SerializeField] private Text distance = null;
    [SerializeField] private Image image = null;
    
    private int time = 0;
    private Vector3 baseLocalPosition;

    private void Start()
    {
        baseLocalPosition = transform.localPosition;
    }

    private void Update()
    {
        if (Player.instance.weaponsEnabled && TargetManager.IsTargetingObject())
            Open();
        else
            Close();

        if (state == State.CLOSED)
            return;

        UpdateDisplay();
        PlayBobbingAnimation();
        HandleOpeningAndClosing();
    }

    private void HandleOpeningAndClosing()
    {
        if (state == State.OPENING)
        {
            status += 1f / ANIMATION_TIME;
            if (status >= 1)
            {
                status = 1;
                state = State.OPENED;
            }
            //GetComponent<Image>().material.SetFloat("_Status", status);
            image.rectTransform.localScale = new Vector3(0.1f * status, 0.1f * status, 0.1f);
        }
        else if (state == State.CLOSING)
        {
            status -= 1f / ANIMATION_TIME;
            if (status <= 0)
            {
                status = 0;
                state = State.CLOSED;
            }
            //GetComponent<Image>().material.SetFloat("_Status", status);
            image.rectTransform.localScale = new Vector3(0.1f * status, 0.1f * status, 0.1f);
        }
    }

    private void UpdateDisplay()
    {
        if (TargetManager.targetedObject != null)
        {
            title.text = TargetManager.targetedObject.name.Replace("(Clone)", "");
            info.text = TargetManager.targetInfo;
            distance.text = "Distance: " + Mathf.Floor(TargetManager.targetDistance) + "m";
        }
    }

    private void Open()
    {
        if (state == State.CLOSED || state == State.CLOSING)
            state = State.OPENING;
    }

    private void Close()
    {
        if (state == State.OPENED || state == State.OPENING)
            state = State.CLOSING;
    }
    
    private void PlayBobbingAnimation()
    {
        time++;
        transform.localPosition = baseLocalPosition + new Vector3(Mathf.Cos(time * 0.0125f) * 0.05f,
                                                                  Mathf.Sin(time * 0.025f) * 0.1f,
                                                                  0);
    }
}
