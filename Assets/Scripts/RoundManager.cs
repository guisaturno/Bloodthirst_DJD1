using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    [SerializeField] internal Transform[] spawnPoints;
    public GameObject enemyPrefab;
    internal int enemysAlive;
    private int roundLevel;
    private float timeResponseAI;

    private void Start()
    {
        enemysAlive = 0;
        timeResponseAI = 2f;
    }

    void Update()
    {
        if (enemysAlive == 0)
        {
            roundLevel += 1;
            if (roundLevel % 2 == 0)
            {
                timeResponseAI -= 0.2f;
                Spawn();
            }
            else
            {
                Spawn();
                Spawn();
            }
        }
    }

    void Spawn()
    {
        int spawnPoint = Random.Range(0, spawnPoints.Length);
        GameObject obj = Instantiate(enemyPrefab, spawnPoints[spawnPoint].position, spawnPoints[spawnPoint].rotation) as GameObject;
        EnemyAI enemyAI = obj.GetComponent<EnemyAI>();
        //enemyAI.NewEnemy();
        enemysAlive += 1;
    }
}
