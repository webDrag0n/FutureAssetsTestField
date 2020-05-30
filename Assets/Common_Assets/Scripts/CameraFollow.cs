using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameObject target;
    public float follow_delta_time;
    public bool follow_pos;
    private Vector3 delta_pos;
    private Quaternion delta_rot;
    // Start is called before the first frame update
    void Start()
    {
        //delta_pos = target.transform.position - transform.position;
        delta_pos = target.transform.position + target.transform.forward * 30 - target.transform.up * 10;
        delta_rot = target.transform.rotation * transform.rotation;
    }

    private void FixedUpdate()
    {
        if (follow_pos)
        {
            //相机的位置
            Vector3 targetPos = target.transform.position - Vector3.up * delta_pos.y + target.transform.forward * delta_pos.x;
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * follow_delta_time);
        }
        //相机的角度
        Quaternion targetRot = Quaternion.LookRotation(target.transform.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * follow_delta_time * 10);
        //transform.LookAt(target.transform.position + target.transform.forward * follow_delta_time + target.transform.up * 2f);

        GetComponent<Camera>().fieldOfView = 60 + target.GetComponent<Rigidbody>().velocity.magnitude / 10f;
    }
    // Update is called once per frame
    void Update()
    {
        //transform.position = target.transform.position - delta_pos;
        //transform.RotateAround(target.transform.position + new Vector3(0, 0, 1), target.transform.rotation.ToEuler(), 1);
        
    }
}
