using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Zero_fighter_control : MonoBehaviour
{
    private Animator animator;
    public GameObject main_engine_flare;
    public GameObject lift_engine_flare;
    public Camera[] cam_array;
    public int current_cam_index;

    public float init_velocity;
    public float engine_power;
    public float air_density;

    private bool jumping;

    public float front_glass_action_time = 1f;

    private Rigidbody rig;
    public TextMeshPro velocity_display;
    public TextMeshPro power_display;
    public TextMeshPro angle_display;
    public TextMeshPro Height_display;
    public TextMeshPro landing_gear_display;

    public bool landing_gear_toggle;
    private float landing_gear_anim_cooldown = 0;
    public bool cockpit_glass_toggle;

    private float mouse_force_x, mouse_force_y;

    // Start is called before the first frame update
    void Start()
    {
        rig = gameObject.GetComponent<Rigidbody>();
        rig.velocity = transform.forward * init_velocity;
        animator = GetComponentInChildren<Animator>();
        main_engine_flare.transform.localScale = new Vector3(1, 1, engine_power / 100000 + 0.3f);
        jumping = false;
    }

    private void Update()
    {
        mouse_force_x = Input.GetAxis("Mouse X") * 50 * Time.deltaTime;
        // inverse y is more comfortable for me
        mouse_force_y = -Input.GetAxis("Mouse Y") * 50 * Time.deltaTime;

        //rig.AddTorque(new Vector3(0, mouse_force_x, mouse_force_y) * 50000);


        if (Input.GetKeyDown(KeyCode.F))
        {
            cockpit_glass_toggle = !cockpit_glass_toggle;
            animator.SetBool("cockpit_glass_toggle", cockpit_glass_toggle);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            cam_array[current_cam_index].gameObject.SetActive(false);
            current_cam_index += 1;
            if (current_cam_index >= cam_array.Length)
            {
                current_cam_index = 0;
            }
            cam_array[current_cam_index].gameObject.SetActive(true);
        }
        if (Input.GetKey(KeyCode.L))
        {
            if (landing_gear_anim_cooldown <= 0)
            {
                landing_gear_toggle = !landing_gear_toggle;
                animator.SetBool("landing_gear_toggle", landing_gear_toggle);
                landing_gear_anim_cooldown = 3.5f;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // all strenth
            engine_power = 300000;
        }


        if (Input.GetKeyDown(KeyCode.X))
        {
            engine_power = 0;
        }

        if (Input.GetKey(KeyCode.Tab))
        {
            // jump
            engine_power = Mathf.Lerp(engine_power, 10000000, 0.01f);

            //jumping = !jumping;
            //if (!jumping)
            //{
            //    engine_power = 0;
            //    rig.velocity = Vector3.zero;
            //} else
            //{
            //    engine_power = 10000000;
            //}
        }else if (Input.GetKeyUp(KeyCode.Tab))
        {
            engine_power = 0;
            rig.velocity = Vector3.zero;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // verticle rotate
        rig.AddForceAtPosition(-transform.up * mouse_force_y * 4000, transform.position + transform.forward * 40);
        rig.AddForceAtPosition(transform.up * mouse_force_y * 4000, transform.position - transform.forward * 20);
        // horizontal rotate
        rig.AddForceAtPosition(transform.right * mouse_force_x * 4000, transform.position + transform.forward * 40);
        rig.AddForceAtPosition(-transform.right * mouse_force_x * 4000, transform.position - transform.forward * 20);


        if (Input.mouseScrollDelta.y > 0)
        {
            if (engine_power < 300000)
            {
                engine_power += 600;
            }
            else
            {
                engine_power = 300000;
            }

        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            if (engine_power > 0)
            {
                engine_power -= 600;
            }
            else
            {
                engine_power = 0;
            }
        }

        if (Input.GetKey(KeyCode.W))
        {
            // acceleration
            rig.AddForce(transform.forward * engine_power * 2);
            main_engine_flare.transform.localScale = new Vector3((engine_power / 300000 + 0.7f + Random.RandomRange(-0.1f, 0.1f)) * 100, 100, 100);
        }
        else
        {
            main_engine_flare.transform.localScale = new Vector3((0.7f + Random.RandomRange(-0.1f, 0.1f)) * 100, 100, 100);
            if (Input.GetKey(KeyCode.S))
            {
                // acceleration
                rig.AddForce(-transform.forward * engine_power * 2);
            }
        }

        if (Input.GetKey(KeyCode.E))
        {
            rig.AddForceAtPosition(-transform.up * 30000, transform.position + transform.right);
            rig.AddForceAtPosition(transform.up * 30000, transform.position - transform.right);
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            rig.AddForceAtPosition(transform.up * 30000, transform.position + transform.right);
            rig.AddForceAtPosition(-transform.up * 30000, transform.position - transform.right);
        }

        if (Input.GetKey(KeyCode.D))
        {
            rig.AddForce(transform.right * 5000);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rig.AddForce(-transform.right * 5000);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            // ascend
            rig.AddForce(transform.up * 15000);
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            // decend
            rig.AddForce(-transform.up * 5000);
        }
        else
        {
            // auto float stablize
            float up_velocity = rig.velocity.magnitude * Vector3.Dot(rig.velocity.normalized, transform.up);
            if (up_velocity < 0.5f)
            {
                rig.AddForce(-up_velocity * transform.up * 15000);
            }
        }

        if (landing_gear_anim_cooldown > 0)
        {
            landing_gear_anim_cooldown -= Time.deltaTime;
        }
        else
        {
            landing_gear_anim_cooldown = 0;
        }

        // calculate physics
        

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
        //rig.AddForceAtPosition(transform.up * Mathf.Pow(forwad_speed, 2) / 1.5f * air_density - resistive_force, transform.position - transform.forward * 0f);
        
        // GUI update
        velocity_display.SetText("" + Mathf.RoundToInt(rig.velocity.magnitude * 1000) / 1000);
        if (engine_power > 300000)
        {
            power_display.SetText("JUMPING");
        }
        else
        {
            power_display.SetText("" + Mathf.RoundToInt(engine_power * 1000) / 300000);
        }
        //angle_display.SetText("Angle: " + counter_angle_deg);
        Height_display.SetText("" + Mathf.RoundToInt(transform.position.y * 100) / 100);
        if (landing_gear_toggle)
        {
            landing_gear_display.SetText("lowered");
        }
        else
        {
            landing_gear_display.SetText("raised");
        }


    }
}
