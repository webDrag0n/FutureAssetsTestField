using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring_local : MonoBehaviour
{
    public GameObject target_obj;
    public Vector3 force_direction;
    private Quaternion rot;
    public string mode;

    private Vector3 previous_position;

    private void Start()
    {
        rot = transform.rotation;
        force_direction = transform.position - target_obj.transform.position;
        previous_position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {


    }

    private void FixedUpdate()
    {
        //GetComponent<Rigidbody>().AddForce(force_direction * )

        //if (mode == "y")
        //{
        //    float magnitude = (transform.position.y - previous_position.y) / force_direction.y;
        //    transform.position -= force_direction * magnitude;

        //}

        transform.rotation = rot;
        //previous_position = transform.position;
        transform.localPosition = new Vector3(0, transform.localPosition.y, 0);
    }

}
