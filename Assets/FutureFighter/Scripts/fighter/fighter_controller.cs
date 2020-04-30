using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class fighter_controller : MonoBehaviour
{
    public Animator front_glass_action;
    public float engine_power;
    public float air_density;

    public float front_glass_action_time = 1f;

    private Rigidbody rig;
    public TextMeshProUGUI velocity_display;
    public TextMeshProUGUI power_display;
    public TextMeshProUGUI angle_display;
    public TextMeshProUGUI Height_display;
    // Start is called before the first frame update
    void Start()
    {
        rig = gameObject.GetComponent<Rigidbody>();
        rig.velocity = transform.forward * 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            front_glass_action.SetTrigger("Start");
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (engine_power < 100000)
            {
                engine_power += 100;
            }
            else
            {
                engine_power = 100000;
            }
        } else if (Input.GetKey(KeyCode.LeftControl))
        {
            if (engine_power > 0)
            {
                engine_power -= 100;
            } else
            {
                engine_power = 0;
            }
        }

        if (Input.GetKey(KeyCode.W))
        {
            rig.AddForceAtPosition(-transform.up * 2, transform.position + transform.forward);
        } else if (Input.GetKey(KeyCode.S))
        {
            rig.AddForceAtPosition(transform.up * 2, transform.position + transform.forward);
        }

        if (Input.GetKey(KeyCode.D))
        {
            rig.AddForceAtPosition(-transform.up * 4, transform.position + transform.right);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rig.AddForceAtPosition(transform.up * 4, transform.position + transform.right);
        }

        // calculate physics
        // acceleration
        rig.AddForce(transform.forward * engine_power * 2);

        //Vector3 self_origin_velocity = new Vector3(
        //    Vector3.Dot(transform.forward, rig.velocity),
        //    Vector3.Dot(transform.right, rig.velocity),
        //    Vector3.Dot(transform.up, rig.velocity) * 2
        //);
        //// air resistance formula
        float air_resistance = Mathf.Pow(rig.velocity.magnitude, 2) / 20 * air_density;
        //Vector3 resistive_force = new Vector3(0, Vector3.Dot(transform.up, rig.velocity) * air_resistance, 0);
        Vector3 resistive_force = rig.velocity * air_resistance;


        float counter_angle_rad = Mathf.Acos(Vector3.Dot(transform.forward, rig.velocity.normalized));
        float counter_angle_deg = counter_angle_rad * 180 / Mathf.PI;

        // downward resistance
        //rig.AddForce(-transform.up * air_resistance * Mathf.Pow(counter_angle_rad, 2));

        // floating force
        float forwad_speed = rig.velocity.magnitude * Mathf.Cos(counter_angle_rad);
        rig.AddForceAtPosition(transform.up * Mathf.Pow(forwad_speed, 2) / 1.5f * air_density - resistive_force, transform.position - transform.forward * 0f);

        // GUI update
        velocity_display.SetText("Velocity: " + rig.velocity.magnitude * 2);
        power_display.SetText("Power: " + engine_power);
        angle_display.SetText("Angle: " + counter_angle_deg);
        Height_display.SetText("Height: " + transform.position.y);
    }
}
