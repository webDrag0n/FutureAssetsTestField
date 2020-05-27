using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using MLAgents.Sensors;

public class Player_AI : Agent
{
    public GameObject football;
    public Transform foe;
    public Transform friend_gate;
    public Transform foe_gate;

    private Vector3 football_pos;
    private Vector3 agent_pos;

    // Start is called before the first frame update
    void Start()
    {
        football_pos = football.transform.localPosition;
        agent_pos = transform.localPosition;
    }


    public override void OnEpisodeBegin()
    {
        football.transform.localPosition = football_pos;
        football.GetComponent<Rigidbody>().velocity = Vector3.zero;
        football.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        transform.localPosition = agent_pos;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(foe.localPosition);
        sensor.AddObservation(transform.localPosition);

        // gate position
        sensor.AddObservation(friend_gate.localPosition);
        sensor.AddObservation(foe_gate.localPosition);

        // football position and velocity
        sensor.AddObservation(football.transform.localPosition);
        sensor.AddObservation(football.GetComponent<Rigidbody>().velocity.x);
        sensor.AddObservation(football.GetComponent<Rigidbody>().velocity.z);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        // always move forward
        // action output: speed
        gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(vectorAction[0] - 5, 0, vectorAction[1] - 5) * 10000);

        // avoid fall off ground
        if (transform.localPosition.y < -0.5f)
        {
            transform.localPosition = agent_pos;
        }

        if (football.transform.localPosition.y < -0.5f)
        {
            football.transform.localPosition = football_pos;
            football.GetComponent<Rigidbody>().velocity = Vector3.zero;
            football.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }


        if (-8f < football.transform.localPosition.z && 8f > football.transform.localPosition.z)
        {
            if (Mathf.Abs(friend_gate.localPosition.x - football.transform.localPosition.x) < 2)
            {
                SetReward(-1);
                EndEpisode();
            }
            else if (Mathf.Abs(foe_gate.localPosition.x - football.transform.localPosition.x) < 2)
            {
                SetReward(1);
                EndEpisode();
            }
        }
        // constant punishment
        SetReward(-1 / 1000f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Football")
        {
            SetReward(0.1f);
        }
    }

    // manual control
    //public override float[] Heuristic()
    //{
    //    var action = new float[2];
    //    if (Input.GetKey(KeyCode.A))
    //    {
    //        action[0] = -5f;
    //    } else if (Input.GetKey(KeyCode.D))
    //    {
    //        action[0] = 5f;
    //    }
    //    action[1] = 10;
    //    return action;
    //}

}
