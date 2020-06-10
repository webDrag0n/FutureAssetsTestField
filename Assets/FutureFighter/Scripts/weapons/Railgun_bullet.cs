using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railgun_bullet : MonoBehaviour
{
    public float life_time;
    private float life_time_count;
    // Start is called before the first frame update
    void Start()
    {
        life_time_count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (life_time_count < life_time)
        {
            life_time_count += Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (life_time_count > 1)
        {
            Destroy(this.gameObject);
        }
    }
}
