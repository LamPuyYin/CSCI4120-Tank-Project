using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoot : MonoBehaviour
{
    public Rigidbody prefabBullet;
    public Rigidbody prefabRocket;

    public float bulletForce;
    public float rocketForce;
    public Transform shootPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Rigidbody instanceBullet = (Rigidbody)Instantiate(prefabBullet, transform.position + shootPosition.forward * 0.8f + Vector3.up * 1.0F, shootPosition.rotation);
            instanceBullet.GetComponent<Rigidbody>().AddForce(shootPosition.forward * bulletForce);
        }
        if (Input.GetButtonDown("Fire2"))
        {
            //Debug.Log("Angle for x axis: " + Mathf.Sin(90.0f + shootPosition.eulerAngles.y) + "with angle as: " + shootPosition.eulerAngles.y);
            Rigidbody instanceRocket1 = (Rigidbody)Instantiate(prefabRocket, transform.position + Vector3.right * 1.7F * Mathf.Sin((90.0f + shootPosition.eulerAngles.y) * Mathf.Deg2Rad) + Vector3.back * 1.7F * Mathf.Sin(shootPosition.eulerAngles.y * Mathf.Deg2Rad) + Vector3.up * 0.5F, shootPosition.rotation);
            instanceRocket1.GetComponent<Rigidbody>().AddForce(shootPosition.forward * rocketForce);
            Rigidbody instanceRocket2 = (Rigidbody)Instantiate(prefabRocket, transform.position + Vector3.right * 3.4F * Mathf.Sin((90.0f + shootPosition.eulerAngles.y) * Mathf.Deg2Rad) + Vector3.back * 3.4F * Mathf.Sin(shootPosition.eulerAngles.y * Mathf.Deg2Rad) + Vector3.up * 0.5F, shootPosition.rotation);
            instanceRocket2.GetComponent<Rigidbody>().AddForce(shootPosition.forward * rocketForce);
            Rigidbody instanceRocket3 = (Rigidbody)Instantiate(prefabRocket, transform.position + Vector3.left * 1.7F * Mathf.Sin((90.0f + shootPosition.eulerAngles.y) * Mathf.Deg2Rad) + Vector3.forward * 1.7F * Mathf.Sin(shootPosition.eulerAngles.y * Mathf.Deg2Rad) + Vector3.up * 0.5F, shootPosition.rotation);
            instanceRocket3.GetComponent<Rigidbody>().AddForce(shootPosition.forward * rocketForce);
            Rigidbody instanceRocket4 = (Rigidbody)Instantiate(prefabRocket, transform.position + Vector3.left * 3.4F * Mathf.Sin((90.0f + shootPosition.eulerAngles.y) * Mathf.Deg2Rad) + Vector3.forward * 3.4F * Mathf.Sin(shootPosition.eulerAngles.y * Mathf.Deg2Rad) + Vector3.up * 0.5F, shootPosition.rotation);
            instanceRocket4.GetComponent<Rigidbody>().AddForce(shootPosition.forward * rocketForce);

        }
    }
}
