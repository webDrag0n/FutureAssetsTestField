using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_control : MonoBehaviour
{
    public float Distance_to_ground;
    public LayerMask layerMask;

    public float walk_speed;
    public float run_speed;
    public float turn_speed;
    private bool is_walking;
    private bool is_running;

    private bool is_grounded;

    private Rigidbody rig;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        is_grounded = false;
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        is_walking = false;
        is_running = false;
        
        if (Input.GetKey(KeyCode.W))
        {
            is_walking = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            //is_walking = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(new Vector3(0, -turn_speed, 0));
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(new Vector3(0, turn_speed, 0));
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (is_walking)
            {
                is_running = true;
            }
        } else
        {
            is_running = false;
        }

    }

    private void FixedUpdate()
    {

        if (is_walking)
        {
            if (is_running)
            {
                animator.SetBool("is_running", true);
                if (is_grounded)
                {
                    transform.position = transform.position + transform.forward * run_speed;
                }
            }
            else
            {
                animator.SetBool("is_running", false);
                animator.SetBool("is_walking", true);
                if (is_grounded)
                {
                    transform.position = transform.position + transform.forward * walk_speed;
                }
            }
        }
        else
        {
            animator.SetBool("is_running", false);
            animator.SetBool("is_walking", false);
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        leave_ground_timer = 0;
        is_grounded = true;
    }
    
    private int leave_ground_timer;
    private void OnCollisionExit(Collision collision)
    {

    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, animator.GetFloat("IKLeftFootWeight"));
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, animator.GetFloat("IKLeftFootWeight"));

            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, animator.GetFloat("IKRightFootWeight"));
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, animator.GetFloat("IKRightFootWeight"));

            // left foot
            RaycastHit hit;
            Ray ray;
            ray = new Ray(animator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);

            if (Physics.Raycast(ray, out hit, Distance_to_ground + 1f, layerMask))
            {
                Vector3 foot_position = hit.point;
                foot_position.y += Distance_to_ground;
                animator.SetIKPosition(AvatarIKGoal.LeftFoot, foot_position);
                animator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.FromToRotation(Vector3.up, hit.normal) * transform.rotation);
            }

            // right foot
            ray = new Ray(animator.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);

            if (Physics.Raycast(ray, out hit, Distance_to_ground + 1f, layerMask))
            {
                Vector3 foot_position = hit.point;
                foot_position.y += Distance_to_ground;
                animator.SetIKPosition(AvatarIKGoal.RightFoot, foot_position);
                animator.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(transform.forward, hit.normal));
            }
        }
    }
}
