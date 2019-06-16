using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    [SerializeField] internal Transform[] spawnPoints;
    [SerializeField] private float baseSpawnTime;
    public GameObject enemyPrefab;
    internal int enemysAlive;
    private int roundLevel;
    private float timeResponseAI;
    private float spawnTime;

    // Crowd sound
    public AudioClip crowd;

    private void Start()
    {
        enemysAlive = 0;
        timeResponseAI = 1.5f;
        spawnTime = baseSpawnTime;
    }

    void Update()
    {
        spawnTime -= Time.deltaTime;
        if (enemysAlive == 0 || spawnTime <= 0 && enemysAlive <= 5)
        {
            roundLevel += 1;
            spawnTime = baseSpawnTime;
            if (roundLevel % 2 == 0 && timeResponseAI >= 0.25)
            {
                timeResponseAI -= 0.25f;
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
        enemyAI.NewEnemy(this, timeResponseAI);
        enemysAlive += 1;

        // Sound crowd
        SoundManager.PlaySound(crowd, Random.Range(0.5f, 1.0f), Random.Range(1.0f, 1.3f));
    }
}
