using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_hit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionExit(Collision collision)
    {
        Destroy(gameObject);
    }
}
