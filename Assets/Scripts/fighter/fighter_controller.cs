using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fighter_controller : MonoBehaviour
{
    public Animator front_glass_action;

    public float front_glass_action_time = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("go");
            front_glass_action.SetTrigger("Start");
        }
    }
}
