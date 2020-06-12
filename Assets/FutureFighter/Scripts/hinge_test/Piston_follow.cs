using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piston_follow : MonoBehaviour
{
    public Transform target;
    public Vector3 piston_distance;
    private Vector3 delta_pos;
    private Quaternion delta_rot;
    
    // Start is called before the first frame update
    void Start()
    {
        piston_distance = Vector3.zero;
        delta_pos = target.position - transform.position;
        delta_rot = target.rotation * transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = target.position - delta_pos - piston_distance;
        transform.LookAt(target, Vector3.up);
    }
}
