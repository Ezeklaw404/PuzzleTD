using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public GameObject[] enemyPrefabs;
        public float spawnTimer;
        public float spawnInterval;
        public int currentWaveWeight;
        public int totalWaveWeight;
        public int enemyCount;
    }

    public Path path;


    public List<Wave> waves;
    public int waveNumber;


    void Update()
    {
        waves[waveNumber].spawnTimer += Time.deltaTime;
        if (waves[waveNumber].totalWaveWeight > waves[waveNumber].currentWaveWeight)
        {

            if (waves[waveNumber].spawnTimer >= waves[waveNumber].spawnInterval)
            {
                waves[waveNumber].spawnTimer = 0;
                SpawnEnemy();
            }
        } else
        {

            if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0)
            {
                waveNumber++;
            }
        }



    }

    private void SpawnEnemy()
    {
        int randEnemyIndex = DetermineEnemiesByWeight();
        GameObject shape = Instantiate(waves[waveNumber].enemyPrefabs[randEnemyIndex],
            transform.position, transform.rotation);

        Enemy enemy = shape.GetComponent<Enemy>();
        if (enemy != null && path != null)
        {
            enemy.InitPath(path.waypoints);
        }
        waves[waveNumber].enemyCount++;
    }



    private int DetermineEnemiesByWeight()
    {
        int spawnIndex = 0;
        int weight;
        int length = waves[waveNumber].enemyPrefabs.Length;

        while (true)
        {
            spawnIndex = Random.Range(0, length);
            weight = waves[waveNumber].enemyPrefabs[spawnIndex].GetComponent<Enemy>().difficultyWeight;

            if ((waves[waveNumber].currentWaveWeight + weight) <= waves[waveNumber].totalWaveWeight)
            {
                waves[waveNumber].currentWaveWeight += weight;
                break;
            }
        }
        return spawnIndex;

    }

}
