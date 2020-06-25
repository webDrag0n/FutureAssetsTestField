using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piston_follow : MonoBehaviour
{
    public Transform target;
    public Transform root;
    

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = root.position;
        transform.LookAt(target);
    }
}
