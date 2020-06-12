using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piston_limit : MonoBehaviour
{

    private Vector3 init_pos;

    // Start is called before the first frame update
    void Start()
    {
        init_pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(0, init_pos.y, 0);
    }
}
