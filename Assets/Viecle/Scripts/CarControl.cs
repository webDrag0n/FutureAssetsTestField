using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}

public enum Mode
{
    land,
    air
}

public class CarControl : MonoBehaviour
{
    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    public Mode mode;
    private Rigidbody rig;

    float motor, steering;

    private void Start()
    {
        rig = GetComponent<Rigidbody>();
        motor = 0;
        steering = 0;
    }

    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    public void WheelRetract(WheelCollider collider, bool isLeft)
    {
        Debug.Log("Switch");
        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        if (isLeft)
        {
            visualWheel.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 60));
        }
        else
        {
            visualWheel.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -60));
        }
    }

    public void WheelExtend(WheelCollider collider)
    {
        Debug.Log("Switch");
        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }
    private void Update()
    {
        motor = maxMotorTorque * Input.GetAxis("Vertical");
        steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (mode == Mode.land)
            {
                mode = Mode.air;
                rig.useGravity = false;
                rig.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                foreach (AxleInfo axleInfo in axleInfos)
                {
                    WheelRetract(axleInfo.leftWheel, true);
                    WheelRetract(axleInfo.rightWheel, false);
                }
            }
            else
            if (mode == Mode.air)
            {
                mode = Mode.land;
                rig.constraints = RigidbodyConstraints.None;
                rig.useGravity = true;
                foreach (AxleInfo axleInfo in axleInfos)
                {
                    WheelExtend(axleInfo.leftWheel);
                    WheelExtend(axleInfo.rightWheel);
                }
            }
        }
    }
    public void FixedUpdate()
    {
        if (mode == Mode.land)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Vector3 velocity = GetComponent<Rigidbody>().velocity;
                motor = -Vector3.Dot(velocity, transform.forward) * rig.mass;
            }

            if (motor == 0)
            {
                Vector3 velocity = GetComponent<Rigidbody>().velocity;
                motor = -Vector3.Dot(velocity, transform.forward) * rig.mass / 30;
            }

            foreach (AxleInfo axleInfo in axleInfos)
            {
                if (axleInfo.steering)
                {
                    axleInfo.leftWheel.steerAngle = steering;
                    axleInfo.rightWheel.steerAngle = steering;
                }
                if (axleInfo.motor)
                {
                    axleInfo.leftWheel.motorTorque = motor;
                    axleInfo.rightWheel.motorTorque = motor;
                }
                ApplyLocalPositionToVisuals(axleInfo.leftWheel);
                ApplyLocalPositionToVisuals(axleInfo.rightWheel);
            }
        }

        if (mode == Mode.air)
        {
            Vector3 velocity = GetComponent<Rigidbody>().velocity;
            float side_stable_force = -Vector3.Dot(velocity, transform.right) * rig.mass;
            float forward_stable_force = -Vector3.Dot(velocity, transform.forward) * rig.mass;
            float upward_stable_force = -Vector3.Dot(velocity, transform.up) * rig.mass;

            if (Input.GetKey(KeyCode.Space))
            {
                float break_magnitude = 2;
                rig.AddForce(
                    transform.forward * forward_stable_force * break_magnitude
                    + transform.up * upward_stable_force * break_magnitude
                    + transform.right * side_stable_force * break_magnitude
                    );
            }
            else
            {
                rig.AddForce(transform.forward * motor + transform.up * upward_stable_force + transform.right * side_stable_force);
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                rig.AddForce(transform.up * rig.mass * 2);
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                rig.AddForce(-transform.up * rig.mass * 2);
            }

            //rig.AddTorque(new Vector3(transform.rotation.eulerAngles.x, steering * 10, transform.rotation.eulerAngles.z));
            float x_stable_torque = 0, z_stable_torque = 0;
            if (transform.rotation.eulerAngles.x > 0)
            {
                x_stable_torque = -1;
            }else if (transform.rotation.eulerAngles.x < 0)
            {
                x_stable_torque = 1;
            }

            if (transform.rotation.eulerAngles.z > 0)
            {
                z_stable_torque = -1;
            }
            else if (transform.rotation.eulerAngles.z < 0)
            {
                z_stable_torque = 1;
            }
            rig.AddTorque(new Vector3(0, steering / 40 * rig.angularDrag, 0) * rig.mass);
            // only allow y axis rotation
            //transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0));
        }
        
    }
}