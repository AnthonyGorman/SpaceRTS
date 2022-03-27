using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Transform enemyPrefab;
    public Transform spawnPoint;

    public float timeInbetweenWaves = 60f;
    private float countdown = 2f;
    private int wavenumber = 1;

    void update()
    {
        if (countdown< 0f)
        {
            SpawnWave();
            countdown = timeInbetweenWaves;
        }
        countdown -= Time.deltaTime;
    }

    IEnumerator SpawnWave()
    {
      for (int i = 0; i < wavenumber; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }
        Debug.Log("Wave Incoming!");
        wavenumber++;
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
