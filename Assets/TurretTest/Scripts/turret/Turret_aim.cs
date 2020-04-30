using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_aim : MonoBehaviour
{
    public GameObject Turret_top;
    public GameObject Turret_gun_base;
    public GameObject Turret_gun_front;
    public GameObject Bullet;

    public float Horizontal_rotation_speed;
    public float Vertical_rotation_speed;

    // match shoot animation time, (3)
    public float cooldown_time;
    float cooldown_count = 0;

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

        // time inverse of whole turret rotation fixed the global and local rotation difference issue
        Vector3 target_direction_to_gun = other.transform.position - transform.position;
        Vector3 target_direction_to_turret_top = other.transform.position - Turret_top.transform.position;
        

        float vertical_angle = 80 - RadToDeg(Mathf.Acos(Vector3.Dot(target_direction_to_gun, transform.forward) / target_direction_to_gun.magnitude));
        float horizontal_angle = RadToDeg(Mathf.Atan2(target_direction_to_turret_top.x, target_direction_to_turret_top.z)) - transform.localEulerAngles.y;
        

        float vertical_rotation = vertical_angle - Turret_gun_base.transform.localRotation.eulerAngles.x;
        // + 360 % 360 to make all angle possitive
        float horizontal_rotation = (horizontal_angle - Turret_top.transform.localRotation.eulerAngles.z + 360) % 360;
        
        // Turret top horizontal rotate
        // if next move will move across the target
        if (Mathf.Abs(horizontal_rotation - 180) > Horizontal_rotation_speed)
        {
            if (horizontal_rotation > 0 && horizontal_rotation < 180)
            {
                Turret_top.transform.localRotation *= Quaternion.Euler(0, 0, -Horizontal_rotation_speed);
            }
            else
            {
                Turret_top.transform.localRotation *= Quaternion.Euler(0, 0, Horizontal_rotation_speed);
            }
        }
        else
        {
            // lock on target
        }

        // Turret gun base vertical rotate
        float next_rotate = Mathf.LerpAngle(Turret_gun_base.transform.localRotation.x, vertical_angle, Time.time / 4);
        if (next_rotate < 70 && next_rotate > -20)
        {
            Turret_gun_base.transform.localRotation = Quaternion.Euler(next_rotate, 0, 0);
        }

        // wait for 3 sec to fire next round
        if (Input.GetKey(KeyCode.Space) && cooldown_count == 0)
        {
            gameObject.GetComponent<Animator>().SetBool("shoot", true);
            Bullet.transform.SetPositionAndRotation(
                Turret_gun_front.transform.position,
                Turret_gun_front.transform.rotation * Quaternion.Euler(-90, 0, 0)
                );
            GameObject b = Instantiate(Bullet);

            b.GetComponent<Rigidbody>().AddForce(Bullet.transform.forward * 10000);

            Debug.Log(Bullet.transform.up * 10000);
            // reset cooldown count
            cooldown_count = cooldown_time;
        }
        else
        {
            gameObject.GetComponent<Animator>().SetBool("shoot", false);
            if (cooldown_count > 0)
            {
                cooldown_count -= Time.deltaTime;
            }
            else
            {
                cooldown_count = 0;
            }
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
