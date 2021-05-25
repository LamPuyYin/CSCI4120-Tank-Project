using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shell : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision otherObj)
    {
        GetComponent<AudioSource>().Play();
        Destroy(gameObject, 0.3f);
    }
}
