using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]

public class MeshDeformer : MonoBehaviour
{
    Mesh planeMesh;
    Vector3[] originalVertices, newVertices, worldPositionVertices;
    Vector3[] vertexDeformSpeed;

    public float expandingRange = 5.0f;
    public float deformationForce = 0.02f;
    private bool collided = false;

    private float deformDuration = 2.0f;
    private float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        planeMesh = GetComponent<MeshFilter>().mesh;
        originalVertices = planeMesh.vertices;
        newVertices = new Vector3[originalVertices.Length];
        worldPositionVertices = new Vector3[originalVertices.Length];
        //Debug.Log("Vertices length" + originalVertices.Length);     


        Matrix4x4 localVertex = transform.localToWorldMatrix;          //make sure collide vertex coordinate has the same vertex as the world

        for (int num = 0; num < originalVertices.Length; num++)
        {
            worldPositionVertices[num] = localVertex.MultiplyPoint3x4(originalVertices[num]);
            newVertices[num] = originalVertices[num];
            //Debug.Log("Vertex created" + newVertices[i].ToString());
        }
        vertexDeformSpeed = new Vector3[originalVertices.Length];
    }

    // Update is called once per frame
    void Update()
    {

        if (collided == true)
        {
            timer += Time.deltaTime;
            if (timer < deformDuration)
            {
                //Debug.Log("deformation start with time as" + timer);
                for (int n = 0; n < newVertices.Length; n++)
                {
                    //update new vertex position
                    Vector3 newVelocityVector = vertexDeformSpeed[n];
                    newVertices[n] += newVelocityVector * Time.deltaTime;
                }
            }
            else
            {
                //Debug.Log("deformation ended");
                timer = 0.0f;
                collided = false;
                for (int n = 0; n < newVertices.Length; n++)
                {
                    vertexDeformSpeed[n] = new Vector3(0, 0, 0);
                }
            }
        }

        planeMesh.vertices = newVertices;
        planeMesh.RecalculateNormals();
    }

    void OnCollisionEnter(Collision hitpoint)
    {
        if (hitpoint.collider.tag == "Rocket")
        {
            ContactPoint contact = hitpoint.GetContact(0);
            Vector3 collisionPosition = contact.point;

            //Debug.Log("Vertex collided" + collisionPosition.ToString());

            AddDeformingForce(collisionPosition);
            collided = true;
        }

    }


    public void AddDeformingForce(Vector3 ContactPoint)
    {
        for (int vertexNumb = 0; vertexNumb < newVertices.Length; vertexNumb++)
        {
            Vector3 positionDifference = worldPositionVertices[vertexNumb] - ContactPoint;
            float influencedForce = deformationForce / (1f + positionDifference.sqrMagnitude);
            float velocity = influencedForce * Time.deltaTime;

            //for spreading out and deforming inwards           


            Deformation_Decisions(ContactPoint, vertexNumb, velocity);

        }
    }

    public void Deformation_Decisions(Vector3 point, int i, float velocity)
    {
        if (worldPositionVertices[i].y < point.y)
        {
            if (worldPositionVertices[i].x < point.x)
            {
                if (Vector3.Distance(point, worldPositionVertices[i]) >= expandingRange)
                {
                    //Debug.Log("Expand!!!");
                    vertexDeformSpeed[i] += new Vector3(-velocity, -velocity, velocity * 10);
                }
                else vertexDeformSpeed[i] += new Vector3(-velocity, -velocity, -velocity * 5);
            }
            else if (worldPositionVertices[i].x > point.x)
            {
                if (Vector3.Distance(point, worldPositionVertices[i]) >= expandingRange)
                {
                    //Debug.Log("Expand!!!");
                    vertexDeformSpeed[i] += new Vector3(-velocity, velocity, velocity * 10);
                }
                else vertexDeformSpeed[i] += new Vector3(-velocity, velocity, -velocity * 5);
            }
        }
        else if (worldPositionVertices[i].y > point.y)
        {
            if (worldPositionVertices[i].x < point.x)
            {
                if (Vector3.Distance(point, worldPositionVertices[i]) >= expandingRange)
                {
                    //Debug.Log("Expand!!!");
                    vertexDeformSpeed[i] += new Vector3(velocity, -velocity, velocity * 10);
                }
                else vertexDeformSpeed[i] += new Vector3(velocity, -velocity, -velocity * 5);
            }
            else if (worldPositionVertices[i].x > point.x)
            {
                if (Vector3.Distance(point, worldPositionVertices[i]) >= expandingRange)
                {
                    //Debug.Log("Expand!!!");
                    vertexDeformSpeed[i] += new Vector3(velocity, velocity, velocity * 10);
                }
                else vertexDeformSpeed[i] += new Vector3(velocity, velocity, -velocity * 5);
            }
        }
        else
            vertexDeformSpeed[i] += new Vector3(0, 0, -velocity * 5);
    }
}
