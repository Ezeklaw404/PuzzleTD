using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private double health;
    [SerializeField] private float moveSpeed = 1;

    [SerializeField]
    public Transform[] waypoints;
    private int waypointIndex = 0;


    public void InitPath(Transform[] path) {
        waypoints = path;
        waypointIndex = 0;
    }

    private void Start()
    {
        // Set position of Enemy as position of the first waypoint
        transform.position = waypoints[waypointIndex].transform.position;
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
        } else
        {
            Destroy(this);
            //TODO deal dmg to health
        }

    }
}
