using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float forwardRate = 3.0F;
    public float turnRate = 2.0F;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float forwardAmount = Input.GetAxis("Vertical") * forwardRate;
        float turnForce = Input.GetAxis("Horizontal") * turnRate;
        transform.Rotate(0, turnForce, 0);
        transform.position += transform.forward * forwardAmount *
        Time.deltaTime;
    }
}
