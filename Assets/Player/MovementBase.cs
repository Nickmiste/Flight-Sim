using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBase : MonoBehaviour
{
    [SerializeField] protected float acceleration;
    [SerializeField] protected float maxSpeed;
    [SerializeField] protected float yawSpeed;
    [SerializeField] protected float pitchSpeed;
    [SerializeField] protected float rollSpeed;

    protected float throttle = 0; //range (0, 1)
    //yaw pitch and roll input, range (-1, 1)
    protected float yaw = 0;
    protected float pitch = 0;
    protected float roll = 0;

    protected void DoMovement()
    {
        if (this.throttle > GetForwardSpeed() / maxSpeed)
            GetComponent<Rigidbody>().AddForce(this.transform.forward * this.acceleration);

        GetComponent<Rigidbody>().AddTorque(this.transform.up * yaw * this.yawSpeed);
        GetComponent<Rigidbody>().AddTorque(this.transform.right * pitch * this.pitchSpeed);
        GetComponent<Rigidbody>().AddTorque(this.transform.forward * roll * this.rollSpeed);
    }

    public float GetSpeed()
    {
        return GetComponent<Rigidbody>().velocity.magnitude;
    }

    public float GetForwardSpeed()
    {
        return GetSpeed() * ((GetComponent<Rigidbody>().velocity.normalized + this.transform.forward.normalized).magnitude / 2);
    }
}