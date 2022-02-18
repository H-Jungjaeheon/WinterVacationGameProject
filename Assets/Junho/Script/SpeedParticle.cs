using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedParticle : MonoBehaviour
{
    ParticleSystemRenderer ps;
    Vector3 left ;
    Vector3 right ;
    // Start is called before the first frame update
    void Start()
    {
        left = new Vector3(0, 0, 0);
        right = new Vector3(180, 0, 0);


        ps = GetComponent<ParticleSystemRenderer>();   
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        if (x < 0)
        {
            ps.flip=left;
        }
        else if (x > 0)
        {
            ps.flip = right;
        }
    }
}
