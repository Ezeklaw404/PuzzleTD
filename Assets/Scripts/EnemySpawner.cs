using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public GameObject[] enemyPrefabs;
        public bool waveOver;
        public float spawnTimer;
        public float spawnInterval;
        public int currentWaveWeight;
        public int totalWaveWeight;
        public int enemyCount;
    }

    public Path path;


    public List<Wave> waves;
    public int waveNumber;

    public int GetWaveNumber() { return waveNumber; }

    void Update()
    {
        if (waveNumber >= waves.Count)
            return;

        waves[waveNumber].spawnTimer += Time.deltaTime;
        if (waves[waveNumber].totalWaveWeight > waves[waveNumber].currentWaveWeight && !waves[waveNumber].waveOver)
        {

            if (waves[waveNumber].spawnTimer >= waves[waveNumber].spawnInterval)
            {
                waves[waveNumber].spawnTimer = 0;
                SpawnEnemy();
            }
        }
        else
        {

            if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0 && waveNumber <= waves.Count - 1)
            {
                waves[waveNumber].waveOver = true;
                waveNumber++;
                Player.Instance.UpdateUI();
            }
        }



    }

    private void SpawnEnemy()
    {
        int randEnemyIndex = DetermineEnemiesByWeight();
        if (randEnemyIndex < 0) return;
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
        int spawnIndex = -1;
        int weight;
        int enemyPrefabCount = waves[waveNumber].enemyPrefabs.Length;
        bool extraWeight = false;
        for (int i = 0; i < enemyPrefabCount; i++)
        {
            int weightOverflow = 0;
            weightOverflow = (waves[waveNumber].enemyPrefabs[i].GetComponent<Enemy>().GetWeight() + waves[waveNumber].currentWaveWeight);
            if (weightOverflow <= waves[waveNumber].totalWaveWeight)
            {
                extraWeight = true;
                break;
            }
        }
        if (!extraWeight) 
        { 
            waves[waveNumber].waveOver = true;
        }

        while (!waves[waveNumber].waveOver)
        {
            spawnIndex = Random.Range(0, enemyPrefabCount);
            weight = waves[waveNumber].enemyPrefabs[spawnIndex].GetComponent<Enemy>().GetWeight();

            if ((waves[waveNumber].currentWaveWeight + weight) <= waves[waveNumber].totalWaveWeight)
            {
                waves[waveNumber].currentWaveWeight += weight;
                break;
            }
        }
        return spawnIndex;

    }

}
