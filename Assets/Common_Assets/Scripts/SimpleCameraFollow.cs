using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCameraFollow : MonoBehaviour
{
    public Transform target;
    public float follow_speed;
    public bool look_at_target;
    public Vector3 look_at_delta;

    private Vector3 delta_pos;
    
    // Start is called before the first frame update
    void Start()
    {
        delta_pos = target.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position - delta_pos, follow_speed);
        if (look_at_target)
        {
            transform.LookAt(target.position + look_at_delta);
        }
    }
}
