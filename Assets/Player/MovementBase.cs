using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBase : MonoBehaviour
{
    [SerializeField] protected float acceleration = 5;
    [SerializeField] protected float maxSpeed = 20;
    [SerializeField] protected float yawSpeed = 1;
    [SerializeField] protected float pitchSpeed = 1;
    [SerializeField] protected float rollSpeed = 1;

    protected float throttle = 0; //range (0, 1)
    //yaw pitch and roll input, range (-1, 1)
    protected float yaw = 0;
    protected float pitch = 0;
    protected float roll = 0;

    protected void DoMovement()
    {
        Rigidbody body = GetComponent<Rigidbody>();

        if (this.throttle > GetForwardSpeed(GetSpeed()) / maxSpeed)
            body.AddForce(this.transform.forward * this.acceleration);

        body.AddTorque(this.transform.up * yaw * this.yawSpeed);
        body.AddTorque(this.transform.right * pitch * this.pitchSpeed);
        body.AddTorque(this.transform.forward * roll * this.rollSpeed);
    }

    public float GetSpeed()
    {
        return GetComponent<Rigidbody>().velocity.magnitude;
    }

    public float GetForwardSpeed(float speed)
    {
        return speed * ((GetComponent<Rigidbody>().velocity.normalized + this.transform.forward.normalized).magnitude / 2);
    }
}