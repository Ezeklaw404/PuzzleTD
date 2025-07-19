using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    //[System.Serializable]
    //public class Wave
    //{
    //    public GameObject enemyPrefab;
    //    public float spawnTimer;
    //    public float spawnInterval;
    //    public int enemyPerWave;
    //    public int enemyCount;
    //}

    //public Path path;


    //public List<Wave> waves;
    //public int waveNumber;


    //void Update()
    //{
    //    waves[waveNumber].spawnTimer += Time.deltaTime;

    //    if (waves[waveNumber].spawnTimer >= waves[waveNumber].spawnInterval)
    //    {
    //        waves[waveNumber].spawnTimer = 0;
    //        SpawnEnemy();
    //    }
    //    if (waves[waveNumber].enemyCount <= 0)
    //    {
    //        //waves[waveNumber].enemyCount++;
    //    }

    //}

    //private void SpawnEnemy()
    //{
    //    GameObject shape = Instantiate(waves[waveNumber].enemyPrefab, transform.position, transform.rotation);

    //    Enemy enemy = shape.GetComponent<Enemy>();
    //    if (enemy != null && path != null)
    //    {
    //       enemy.InitPath(path.waypoints);
    //    }
    //    waves[waveNumber].enemyCount++;
    //}




    public GameObject enemyPrefab;
    public float spawnTimer;
    public float spawnInterval;
    public int enemyPerWave;
    public int enemyCount;

    public Path path;




    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0;
            SpawnEnemy();
        }
        if (enemyCount <= 0)
        {

        }

    }

    private void SpawnEnemy()
    {
        GameObject shape = Instantiate(enemyPrefab, transform.position, transform.rotation);

        Enemy enemy = shape.GetComponent<Enemy>();
        if (enemy != null && path != null)
        {
            enemy.InitPath(path.waypoints);
        }
        enemyCount++;
    }

}
