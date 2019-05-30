using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MovementBase
{
    public static PlayerMovement instance { get; private set; }

    public enum ControlScheme { JOYSTICK, MOUSE_AND_KEYBOARD, KEYBOARD, DEBUG }

    [SerializeField] private float enginePitch = 2;
    [SerializeField] private ControlScheme controlScheme;
    [SerializeField] private float yawPitchMod = 0; //multiply yawSpeed and pitchSpeed by this when using ControlScheme.MOUSE_AND_KEYBOARD

    private float baseYawSpeed;
    private float basePitchSpeed;

    [SerializeField] private float throttle_sens = 0.0175f;
    [SerializeField] private float mouse_sens = 0.03f;
    [SerializeField] private float ypr_sens = 0.02f;
    [SerializeField] private float ypr_decay = 0.01f;

    private bool controlsEnabled = true;

    private void Start()
    {
        instance = this;
        this.baseYawSpeed = this.yawSpeed;
        this.basePitchSpeed = this.pitchSpeed;
        this.SetControlScheme(controlScheme);
    }

    private void FixedUpdate()
    {
        if (controlsEnabled)
        {
            UpdateThrottle();
            UpdateYPR();
        }

        DoMovement();

        float speed = GetSpeed();
        GetComponent<AudioSource>().pitch = Mathf.Lerp(1, enginePitch, speed / maxSpeed);
        UpdateUI(speed, GetForwardSpeed(speed));
    }

    private void UpdateThrottle()
    {
        if (controlScheme == ControlScheme.JOYSTICK)
            this.throttle = (Input.GetAxis("Throttle") + 1) / 2;
        else if (controlScheme == ControlScheme.MOUSE_AND_KEYBOARD)
        {
            if (Input.GetButton("ThrottleUp"))
                this.throttle += throttle_sens;
            if (Input.GetButton("ThrottleDown"))
                this.throttle -= throttle_sens;
        }
        else if (controlScheme == ControlScheme.KEYBOARD)
        {
            if (Input.GetButton("ThrottleUpKeyboard"))
                this.throttle += throttle_sens;
            if (Input.GetButton("ThrottleDownKeyboard"))
                this.throttle -= throttle_sens;
        }
        else if (controlScheme == ControlScheme.DEBUG)
            this.throttle = 0;
        this.throttle = Mathf.Clamp01(this.throttle);
    }

    private void UpdateYPR()
    {
        if (controlScheme == ControlScheme.JOYSTICK)
        {
            this.yaw = Input.GetAxis("Yaw");
            this.pitch = Input.GetAxis("Pitch");
            this.roll = Input.GetAxis("Roll");
        }
        else if (controlScheme == ControlScheme.MOUSE_AND_KEYBOARD || controlScheme == ControlScheme.DEBUG)
        {
            //yaw and pitch
            this.yaw = Input.GetAxis("Horizontal") * mouse_sens;
            this.pitch = Input.GetAxis("Vertical") * mouse_sens;

            //roll (same code from ControlScheme.KEYBOARD)
            if (Input.GetButton("RollUp"))
                this.roll += ypr_sens;
            if (Input.GetButton("RollDown"))
                this.roll -= ypr_sens;
            this.roll -= ypr_decay * Mathf.Sign(this.roll);
            if (Mathf.Abs(this.roll) < ypr_decay)
                this.roll = 0;

            //clamp
            this.yaw = Mathf.Clamp(this.yaw, -1, 1);
            this.pitch = Mathf.Clamp(this.pitch, -1, 1);
            this.roll = Mathf.Clamp(this.roll, -1, 1);
        }
        else if (controlScheme == ControlScheme.KEYBOARD)
        {
            if (Input.GetButton("YawUp"))
                this.yaw += ypr_sens;
            if (Input.GetButton("YawDown"))
                this.yaw -= ypr_sens;
            if (Input.GetButton("PitchUp"))
                this.pitch += ypr_sens;
            if (Input.GetButton("PitchDown"))
                this.pitch -= ypr_sens;
            if (Input.GetButton("RollUpKeyboard"))
                this.roll += ypr_sens;
            if (Input.GetButton("RollDownKeyboard"))
                this.roll -= ypr_sens;

            //decay
            this.yaw -= ypr_decay * Mathf.Sign(this.yaw);
            this.pitch -= ypr_decay * Mathf.Sign(this.pitch);
            this.roll -= ypr_decay * Mathf.Sign(this.roll);
            if (Mathf.Abs(this.yaw) < ypr_decay) this.yaw = 0;
            if (Mathf.Abs(this.pitch) < ypr_decay) this.pitch = 0;
            if (Mathf.Abs(this.roll) < ypr_decay) this.roll = 0;

            //clamp
            this.yaw = Mathf.Clamp(this.yaw, -1, 1);
            this.pitch = Mathf.Clamp(this.pitch, -1, 1);
            this.roll = Mathf.Clamp(this.roll, -1, 1);
        }
    }

    public void Reset()
    {
        throttle = 0;
        yaw = 0;
        pitch = 0;
        roll = 0;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        GetComponent<Rigidbody>().position = Vector3.zero;
    }

    public void ResetInput()
    {
        throttle = 0;
        yaw = 0;
        pitch = 0;
        roll = 0;
    }

    public void SetControlsEnabled(bool enabled)
    {
        controlsEnabled = enabled;
        if (!enabled)
            ResetInput();
    }

    private void DoDebugMovement()
    {
        GetComponent<Rigidbody>().position += transform.forward * Input.GetAxis("DebugForward");
        GetComponent<Rigidbody>().position += transform.up * Input.GetAxis("DebugUp");
        GetComponent<Rigidbody>().position += transform.right * Input.GetAxis("DebugRight");
    }

    private void UpdateUI(float speed, float forwardSpeed)
    {
        Speedometer.UpdateSpeedometerUI(this.throttle, speed / maxSpeed, forwardSpeed / maxSpeed);
    }
    
    private void SetControlScheme(ControlScheme controlScheme)
    {
        bool mouse = controlScheme == ControlScheme.MOUSE_AND_KEYBOARD || controlScheme == ControlScheme.DEBUG;
        this.controlScheme = controlScheme;
        Cursor.lockState = mouse ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !mouse;

        this.yawSpeed = this.baseYawSpeed;
        this.pitchSpeed = this.basePitchSpeed;
        if (mouse)
        {
            this.yawSpeed *= this.yawPitchMod;
            this.pitchSpeed *= this.yawPitchMod;
        }
    }

    public ControlScheme GetControlScheme() => controlScheme;
}