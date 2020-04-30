using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveInitialForce : MonoBehaviour
{
    public Vector3 Force_vector;
    public bool is_impulse;
    // Start is called before the first frame update
    void Start()
    {
        if (is_impulse)
        {
            gameObject.GetComponent<Rigidbody>().AddForce(Force_vector, ForceMode.Impulse);
        }
        else
        {
            gameObject.GetComponent<Rigidbody>().AddForce(Force_vector);
        }
    }
    

    private void OnCollisionEnter(Collision collision)
    {
        Time.timeScale = 0.5f;
    }
}
