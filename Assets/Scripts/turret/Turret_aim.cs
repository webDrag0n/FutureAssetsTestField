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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.GetComponent<Animator>().SetBool("shoot", true);
        }

        float horizontal_angle = Mathf.Atan2(other.transform.position.x - transform.position.x, other.transform.position.z - transform.position.z);

        Debug.Log(Mathf.LerpAngle(Turret_top.transform.rotation.y, (horizontal_angle) + 180, 1f));
        Turret_top.transform.Rotate(new Vector3(0, 1, 0), Mathf.LerpAngle(Turret_top.transform.rotation.y, (horizontal_angle) + 180, 1f));

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
