using UnityEngine;

public class Path : MonoBehaviour
{

    public Transform[] waypoints;

    public Transform GetWaypoint(int index)
    {
        if (index < waypoints.Length)
            return waypoints[index];
        return null;
    }

    public int Length => waypoints.Length;
}
