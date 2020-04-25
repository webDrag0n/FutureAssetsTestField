using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeControl : MonoBehaviour
{
    // front left floating point
    [SerializeField] GameObject flfp;
    // front right floating point
    [SerializeField] GameObject frfp;
    // back floating point
    [SerializeField] GameObject bfp;
    // teleporting shader
    [SerializeField] Material teleport_mat;
    // used to replace the real ship to provide teleport effect
    [SerializeField] GameObject FloatBike_teleporting;
    // main engine flare
    //[SerializeField] GameObject main_engine_flare;
    //[SerializeField] GameObject flfp_engine_flare;
    //[SerializeField] GameObject frfp_engine_flare;

    private float main_engine_output;
    private float flfp_engine_output;
    private float frfp_engine_output;
    private float bfp_engine_output;

    private float mass;

    public float preset_flight_height;
    private float delta_height;
    public float main_engine_max_output;
    private float turning_force;
    
    private Vector3 teleport_target;

    private int teleport_time_count = 0;
    private Vector3 velocity_before_teleport;

    // Start is called before the first frame update
    void Start()
    {
        delta_height = 0;
        main_engine_output = 0;
        flfp_engine_output = 0;
        frfp_engine_output = 0;
        bfp_engine_output = 0;

        mass = GetComponent<Rigidbody>().mass;

        // + for right turn, - for left
        turning_force = 0;

        velocity_before_teleport = new Vector3(0, 0, 0);
        FloatBike_teleporting.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (teleport_time_count > 0)
        {
            // put teleportation effect model
            FloatBike_teleporting.transform.position = transform.position;
            FloatBike_teleporting.transform.rotation = transform.rotation;
        }

        main_engine_output = (main_engine_max_output - gameObject.GetComponent<Rigidbody>().velocity.magnitude);
        if (main_engine_output < 0)
        {
            main_engine_output = 0;
        }
        if (Input.GetKey(KeyCode.W))
        {
            gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * main_engine_output * mass);
        }
        if (Input.GetKey(KeyCode.S))
        {
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            preset_flight_height += 0.1f;
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            preset_flight_height -= 0.1f;
        }

        // rotate force (no engine)
        turning_force = 0;
        if (Input.GetKey(KeyCode.D))
        {
            //gameObject.GetComponent<Rigidbody>().AddForceAtPosition(8f * transform.right, transform.position + 2 * transform.forward - 0.2f * transform.up);
            turning_force += 10f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            //gameObject.GetComponent<Rigidbody>().AddForceAtPosition(-8f * transform.right, transform.position + 2 * transform.forward - 0.2f * transform.up);
            turning_force -= 10f;
        }

        // disable teleport during teleport
        if (Input.GetKeyDown(KeyCode.Q) && teleport_time_count == 0)
        {
            teleport_time_count = 1;

            velocity_before_teleport = transform.GetComponent<Rigidbody>().velocity;

            FloatBike_teleporting.transform.position = transform.position;
            FloatBike_teleporting.transform.rotation = transform.rotation;
            FloatBike_teleporting.SetActive(true);

        }
        
        // adjust engine flare effect by resizing them
        //main_engine_flare.transform.localScale = new Vector3(1, 1.5f - main_engine_output / main_engine_max_output, 1);
        //flfp_engine_flare.transform.localScale = new Vector3(1, 0.8f + flfp_engine_output / 15, 1);
        //frfp_engine_flare.transform.localScale = new Vector3(1, 0.8f + frfp_engine_output / 15, 1);
    }

    private void FixedUpdate()
    {
        if (teleport_time_count > 0)
        {
            // start teleportation count
            teleport_time_count += 1;
        }
        // teleportation
        if (teleport_time_count > 40)
        {
            teleport_time_count = 0;
            FloatBike_teleporting.SetActive(false);
        }
        if (teleport_time_count == 20)
        {
            // teleport when time reach limit
            teleport_target = transform.position + new Vector3(transform.forward.x, 0, transform.forward.z) * 100f;
            transform.position = teleport_target;

            transform.GetComponent<Rigidbody>().velocity = velocity_before_teleport;

        }
        else
        {
            // not in teleportation mode
            // do normal interations
            delta_height = preset_flight_height - transform.position.y;
            // front left engine
            flfp_engine_output = 14f + 5f * (preset_flight_height - flfp.transform.position.y) + turning_force;
            gameObject.GetComponent<Rigidbody>().AddForceAtPosition(-flfp.transform.right * flfp_engine_output * mass, flfp.transform.position);

            // front right engine
            frfp_engine_output = 14f + 5f * (preset_flight_height - frfp.transform.position.y) - turning_force;
            gameObject.GetComponent<Rigidbody>().AddForceAtPosition(-frfp.transform.right * frfp_engine_output * mass, frfp.transform.position);

            // back engine
            bfp_engine_output = 9.8f + 2.5f * (preset_flight_height - bfp.transform.position.y) - Mathf.Abs(turning_force * 0.05f);

            gameObject.GetComponent<Rigidbody>().AddForceAtPosition(new Vector3(0, 10, 0) * bfp_engine_output * mass, bfp.transform.position);
        }
        
    }
}
