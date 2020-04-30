using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameObject target;
    public float follow_delta_time;
    private Vector3 delta_pos;
    // Start is called before the first frame update
    void Start()
    {
        delta_pos = target.transform.position - transform.position;
    }

    private void FixedUpdate()
    {
        //相机的位置
        Vector3 targetPos = target.transform.position - Vector3.up * delta_pos.y * 1f - target.transform.forward * delta_pos.z;
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * follow_delta_time);
        //相机的角度
        transform.LookAt(target.transform.position + target.transform.forward * follow_delta_time + target.transform.up * 2f);

        GetComponent<Camera>().fieldOfView = 60 + target.GetComponent<Rigidbody>().velocity.magnitude / 10f;
    }
    // Update is called once per frame
    void Update()
    {
        //transform.position = target.transform.position - delta_pos;
        //transform.RotateAround(target.transform.position + new Vector3(0, 0, 1), target.transform.rotation.ToEuler(), 1);
        
    }
}
