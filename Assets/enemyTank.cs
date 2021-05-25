using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyTank : MonoBehaviour
{
    public Transform goal;
    public int lifePoint = 300;
    private UnityEngine.AI.NavMeshAgent agent;

    
    private int attackState;

    public float rotationDamping = 6.0f;

    public Rigidbody prefabBullet;
    public float shootForce;
    public Transform shootPosition;


    public float sightAngle = 120.0f;
    public float sightRadius = 30.0f;

    public LayerMask targetLayout;
	public LayerMask obstacleLayout;


    public enum Behavior
    {
        GoalSeeking,
        Patrol
    };
    public Behavior GameStysle;

    public Transform[] partolPt;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        switch (GameStysle)
        {
            case Behavior.GoalSeeking:
                agent.destination = goal.position;
                break;
            case Behavior.Patrol:
                agent.destination = partolPt[0].position;
                break;
        }
        
        StartCoroutine("attackOrMove");
    }

    // Update is called once per frame
    void Update()
    {
        GameObject playerTank;
        playerTank = GameObject.FindGameObjectWithTag("Player");

        Collider[] targetsSeen = Physics.OverlapSphere(transform.position, sightRadius, targetLayout);

        for (int listN = 0; listN < targetsSeen.Length; listN++)
        {
            if (targetsSeen[listN].transform == playerTank.transform)
            {
                Transform enemyTarget = playerTank.transform;
                Vector3 enemyTargetDirection = (enemyTarget.position - transform.position).normalized;

                Vector3 sightDistance = (enemyTarget.transform.position - transform.position);
                if ((Vector3.Angle(transform.forward, enemyTargetDirection) < (sightAngle / 2)) && (sightDistance.sqrMagnitude < sightRadius * sightRadius))
                {
                    float targetDistanceVect = Vector3.Distance(transform.position, enemyTarget.position);

                    if (!Physics.Raycast(transform.position, enemyTargetDirection, targetDistanceVect, obstacleLayout))
                    {
                        //Debug.Log("Spotted Player");
                        attackState = 1;
                        agent.isStopped = true;
                    }
                    else
                    {
                        //Debug.Log("Player blocked from obstacles");
                        attackState = 0;
                        agent.isStopped = false;
                    }
                }
                else
                {
                    //Debug.Log("Player not in visible sector or too far");
                    attackState = 0;
                    agent.isStopped = false;
                }
            }
            else {
                //Debug.Log("Player not in visible range");
                attackState = 0;
                agent.isStopped = false;
            }
        }

        if (attackState == 1)
        {
            Quaternion rotation = Quaternion.LookRotation(playerTank.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationDamping);
        }
    }

    IEnumerator attackOrMove()
    {
        GameObject enemy;
        while (true)
        {
            
            if (attackState == 0)
            {
                switch (GameStysle)
                {
                    case Behavior.GoalSeeking:
                        agent.destination = goal.position;
                        break;
                    case Behavior.Patrol:
                        //Debug.Log("Enemy tank on the move");
                        Vector3 partolPtArea1 = (transform.position - partolPt[0].position);
                        Vector3 partolPtArea2 = (transform.position - partolPt[1].position);
                        Vector3 partolPtArea3 = (transform.position - partolPt[2].position);
                        if (partolPtArea1.sqrMagnitude < 2 )
                            agent.destination = partolPt[1].position;
                        else if (partolPtArea2.sqrMagnitude < 2)
                            agent.destination = partolPt[2].position;
                        else if (partolPtArea3.sqrMagnitude < 2)
                            agent.destination = partolPt[0].position;
                        break;
                }
                
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                enemy = GameObject.FindWithTag("Player");
                transform.LookAt(enemy.transform.position);
                Rigidbody instanceBullet = Instantiate(prefabBullet, shootPosition.position + shootPosition.forward * 0.8f + Vector3.up * 0.3F, shootPosition.rotation);
                instanceBullet.GetComponent<Rigidbody>().AddForce(shootPosition.forward * shootForce);
                yield return new WaitForSeconds(1.0f);
            }
        }
    }

    private void OnCollisionEnter(Collision otherObj)
    {
        if (otherObj.collider.tag == "Shell")
        {
            lifePoint -= 10;
            if (lifePoint <= 0) Destroy(gameObject, 0.5F);
        }
    }


}
