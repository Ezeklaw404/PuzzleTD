using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public double health;
    [SerializeField] public float moveSpeed = 1;
    [SerializeField] private int difficultyWeight = 1;
    public bool preserveSpawnPosition = false;

    [SerializeField]
    public Transform[] waypoints;
    public int waypointIndex = 0;

    public static readonly List<Enemy> AllEnemies = new List<Enemy>();

    void OnEnable() => AllEnemies.Add(this);
    void OnDisable() => AllEnemies.Remove(this);

    public static event Action<Enemy> OnDeath;


    public void InitPath(Transform[] path) {
        waypoints = path;
        waypointIndex = 0;
    }

    public void TakeDamage(double damage)
    {
        health -= damage;
        if (health <= 0)
        {
            EnemyDie();
        }
    }

    public int GetWeight()
    {
        return difficultyWeight;
    }

    private void Start()
    {

        if (!preserveSpawnPosition)
        { 
            transform.position = waypoints[waypointIndex].transform.position;
        }
    }


    private void Update()
    {
        Move();
    }

    // Method that actually make Enemy walk
    private void Move()
    {
        // If Enemy didn't reach last waypoint it can move
        // If enemy reached last waypoint then it stops
        if (waypointIndex <= waypoints.Length - 1)
        {

            // Move Enemy from current waypoint to the next one
            // using MoveTowards method
            transform.position = Vector2.MoveTowards(transform.position,
               waypoints[waypointIndex].transform.position,
               moveSpeed * Time.deltaTime);

            // If Enemy reaches position of waypoint he walked towards
            // then waypointIndex is increased by 1
            // and Enemy starts to walk to the next waypoint
            if (transform.position == waypoints[waypointIndex].transform.position)
            {
                waypointIndex += 1;
            }
        }
    }

    virtual protected void EnemyDie()
    {
        OnDeath?.Invoke(this);
        Destroy(gameObject);
    }

    void OnCollisionStay2D(Collision2D collision) 
    {
        if (collision.gameObject.CompareTag("TrackEnd"))
        {
            if (Player.Instance != null)
            {
                //int dmg = difficultyWeight / 2;
                //if (dmg < 1) dmg = 1;
                Player.Instance.Yeouch(GetWeight());
            }
            Destroy(gameObject);
        }
    }
}