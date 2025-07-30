using System;
using System.Collections.Generic;
using UnityEngine;

public class Splitter : Enemy
{

    protected override void EnemyDie()
    {
        //split the enemy into two smaller enemies that have half the health of the original by making them enemy prefab min_split
        GameObject enemyPrefab = Resources.Load<GameObject>("min_split");
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
        script1.health = 7.5f;
        script1.moveSpeed = moveSpeed * 0.8f;
        script1.waypoints = waypoints;
        script1.waypointIndex = waypointIndex;

        // Configure enemy2
        var script2 = enemy2.GetComponent<Enemy>();
        script2.preserveSpawnPosition = true;
        script2.health = 7.5f;
        script2.moveSpeed = moveSpeed * 0.8f;
        script2.waypoints = waypoints;
        script2.waypointIndex = waypointIndex;
        base.EnemyDie();
    }
}