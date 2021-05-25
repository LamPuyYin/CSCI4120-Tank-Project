using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject spawnTank;
    public Transform[] spawnPt;
    public int enemyTotal = 10;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("spawnEnemy");
    }

    IEnumerator spawnEnemy()
    {
        int pt;
        for (int i = 0; i < enemyTotal; i++)
        {
            pt = (int)Random.Range(0.0f, 2.99f);
            var instanceTank = Instantiate(spawnTank, spawnPt[pt].position, spawnPt[pt].rotation);
            yield return new WaitForSeconds(10.0f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
