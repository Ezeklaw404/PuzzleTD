using System;
using System.Collections.Generic;
using UnityEngine;

public class Splitter : Enemy
{
    public GameObject enemyPrefab;

    protected override void EnemyDie()
    {
        //split the enemy into two smaller enemies that have half the health of the original by making them enemy prefab min_split
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy prefab 'min_split' not found in Resources.");
            return;
        }

        Vector3 deathPosition = this.transform.position;

        GameObject enemy1 = Instantiate(enemyPrefab, deathPosition + new Vector3(-0.1f, -0.1f, 0), Quaternion.identity);
        GameObject enemy2 = Instantiate(enemyPrefab, deathPosition + new Vector3(0.1f, 0.1f, 0), Quaternion.identity);

        // Configure enemy1
        var script1 = enemy1.GetComponent<Enemy>();
        script1.preserveSpawnPosition = true;
        ////script1.health = script1.GetHealth();
        ////script1.moveSpeed = moveSpeed;
        script1.waypoints = waypoints;
        script1.waypointIndex = waypointIndex;
        ////script1.SetWeight(script1.GetWeight());

        //// Configure enemy2
        var script2 = enemy2.GetComponent<Enemy>();
        script2.preserveSpawnPosition = true;
        //script2.health = script2.GetHealth();
        //script2.moveSpeed = moveSpeed;
        script2.waypoints = waypoints;
        script2.waypointIndex = waypointIndex;
        //script2.SetWeight(script2.GetWeight());
        base.EnemyDie();
    }
}