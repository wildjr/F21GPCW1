using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Bounds gameArea;
    public GameObject healthPrefab;
    public int healthOrbsToSpawnMin;
    public int healthOrbsToSpawnMax;

    public Bounds[] enemySpawnArea;
    public GameObject enemyPrefab;
    public int enemiesToSpawnMin;
    public int enemiesToSpawnMax;

    // Start is called before the first frame update
    void Start()
    {
        int healthOrbsToSpawn = Random.Range(healthOrbsToSpawnMin, healthOrbsToSpawnMax);

        for (int i=0; i<healthOrbsToSpawn; i++)
        {
            GameObject.Instantiate(healthPrefab, new Vector3(
                Random.Range(gameArea.min.x, gameArea.max.x),
                Random.Range(gameArea.min.y, gameArea.max.y),
                Random.Range(gameArea.min.z, gameArea.max.z)), transform.rotation);
        }

        int enemiesToSpawn = Random.Range(enemiesToSpawnMin, enemiesToSpawnMax);

        for (int i=0; i<enemiesToSpawn; i++)
        {
            GameObject.Instantiate(enemyPrefab, new Vector3(
                Random.Range(enemySpawnArea[i % enemySpawnArea.Length].min.x, enemySpawnArea[i % enemySpawnArea.Length].max.x),
                Random.Range(enemySpawnArea[i % enemySpawnArea.Length].min.y, enemySpawnArea[i % enemySpawnArea.Length].max.y),
                Random.Range(enemySpawnArea[i % enemySpawnArea.Length].min.z, enemySpawnArea[i % enemySpawnArea.Length].max.z)), transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
