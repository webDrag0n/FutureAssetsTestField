using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_aim : MonoBehaviour
{
    public GameObject Turret_top;
    public GameObject Turret_gun_base;
    public GameObject Turret_gun_back;
    public GameObject Turret_gun_front;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "target")
        {
            return;
        }
        // stop playing idle animation and transit to shoot ready stage
        gameObject.GetComponent<Animator>().SetBool("target_found", true);
        

        Vector3 target_direction_to_gun = other.transform.position - Turret_gun_base.transform.position;
        Vector3 target_direction_to_turret_top = other.transform.position - Turret_top.transform.position;

        float horizontal_angle = RadToDeg(
            Mathf.Atan2(target_direction_to_turret_top.x,
            target_direction_to_turret_top.z)
            ) + 180;

        // cos(theta) = a*b / (|a|*|b|)
        float vertical_angle = 90 - RadToDeg(
            Mathf.Acos(Vector3.Dot(transform.up, target_direction_to_gun) / target_direction_to_gun.magnitude)
            );

        float horizontal_rotation = horizontal_angle - Turret_top.transform.localRotation.y;

        //float horizontal_delta_rotation = 0;
        //// check if target angle is at the limit of going to far
        //if (Mathf.Abs(horizontal_rotation) > 2)
        //{
        //    horizontal_delta_rotation = horizontal_rotation % 1f;

        //    
        //}
        //else
        //{
        //}
        Turret_top.transform.localRotation = Quaternion.Euler(0, 0, horizontal_rotation);
            

        Turret_gun_base.transform.localRotation = Quaternion.Euler(Mathf.LerpAngle(Turret_gun_base.transform.rotation.x, vertical_angle, Time.time / 4), 0, 0);

        if (Input.GetKey(KeyCode.Space))
        {
            gameObject.GetComponent<Animator>().SetBool("shoot", true);
        }
        else
        {
            gameObject.GetComponent<Animator>().SetBool("shoot", false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // continue playing idle animation
        gameObject.GetComponent<Animator>().SetBool("target_found", false);
    }

    float RadToDeg(float rad)
    {
        return rad * 180 / Mathf.PI;
    }
}
