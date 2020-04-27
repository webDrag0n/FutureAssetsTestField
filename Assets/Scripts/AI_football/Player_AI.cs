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
        sensor.AddObservation(football.transform.localPosition);
        sensor.AddObservation(foe.localPosition);
        sensor.AddObservation(transform.localPosition);

        // gate position
        sensor.AddObservation(friend_gate.localPosition);
        sensor.AddObservation(foe_gate.localPosition);

        // football velocity
        sensor.AddObservation(football.GetComponent<Rigidbody>().velocity.x);
        sensor.AddObservation(football.GetComponent<Rigidbody>().velocity.z);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        // always move forward
        gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 8000);
        // only one action output: the angle to rotate
        transform.Rotate(new Vector3(0, vectorAction[0] - 5, 0));

        // avoid fall off ground
        if (transform.localPosition.y < 0)
        {
            EndEpisode();
        }

        // reward and punishment
        if (-8f < football.transform.localPosition.z && 8f > football.transform.localPosition.z)
        {
            //if (friend_gate.localPosition.x < football.transform.localPosition.x)
            //{
            //    SetReward(-1);
            //    EndEpisode();
            //} else if (foe_gate.localPosition.x > football.transform.localPosition.x)
            //{
            //    SetReward(1);
            //    EndEpisode();
            //}
        }
        Debug.Log(football.transform.localPosition.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            SetReward(-0.5f);
            //EndEpisode();
        }
        return;
    }

    // manual control
    public override float[] Heuristic()
    {
        var action = new float[1];
        if (Input.GetKey(KeyCode.A))
        {
            action[0] = 0f;
        } else if (Input.GetKey(KeyCode.D))
        {
            action[0] = 10f;
        }
        return action;
    }

}
