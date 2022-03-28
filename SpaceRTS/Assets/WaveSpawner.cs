using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Transform enemyPrefab;
    public Transform spawnPoint;

    public float timeInbetweenWaves = 60f;
    public float countdown = 2f;
    private int wavenumber = 1;

    void Update()
    {
        if (countdown< 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeInbetweenWaves;
        }
        countdown -= Time.deltaTime;
    }

    IEnumerator SpawnWave()
    {
        //Debug.Log("Wave Incoming!");
        for (int i = 0; i < wavenumber; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
            Debug.Log("SPAWN");
        }
        wavenumber++;
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        
    }
}
