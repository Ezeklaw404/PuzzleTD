using System;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public enum TargetPriority
    {
        Random,
        Closest,
        Farthest,
        HighestHealth,
        LowestHealth,
        MostProgressed,
        LeastProgressed
    }

    [SerializeField] private TargetPriority priority;
    [SerializeField] private float range;
    [Tooltip("Shots per minute")]
    [SerializeField] private double fireRate;
    [SerializeField] private double damage;
    [SerializeField] private byte targetCount;
    private CircleCollider2D rangeCollider;
    private List<Collider2D> targetsInRange = new();
    private float fireCooldown = 0f;

    void Start()
    {
        rangeCollider = gameObject.AddComponent<CircleCollider2D>();
        rangeCollider.isTrigger = true;
        rangeCollider.radius = range;
    }

    void Update()
    {
        rangeCollider.Overlap(
            new ContactFilter2D().NoFilter(),
            targetsInRange
        );

        targetsInRange.RemoveAll(c => c == null || !c.CompareTag("Enemy"));

        if (targetsInRange.Count > 0)
        {
            fireCooldown -= Time.deltaTime;
            if (fireCooldown <= 0f)
            {
                FireAtTargets();
                fireCooldown = 60f / (float)fireRate;
            }
        }
    }

    private void SortTargets()
    {
        Vector2 myPos = transform.position;

        switch (priority)
        {
            case TargetPriority.Random:
                // Fisher–Yates shuffle
                for (int i = targetsInRange.Count - 1; i > 0; i--)
                {
                    int j = UnityEngine.Random.Range(0, i + 1);
                    (targetsInRange[i], targetsInRange[j]) =
                      (targetsInRange[j], targetsInRange[i]);
                }
                break;

            case TargetPriority.Closest:
                targetsInRange.Sort((a, b) =>
                    Vector2.Distance(a.transform.position, myPos)
                         .CompareTo(Vector2.Distance(b.transform.position, myPos)));
                break;

            case TargetPriority.Farthest:
                targetsInRange.Sort((a, b) =>
                    Vector2.Distance(b.transform.position, myPos)
                         .CompareTo(Vector2.Distance(a.transform.position, myPos)));
                break;

            case TargetPriority.HighestHealth:
                targetsInRange.Sort((a, b) =>
                    b.GetComponent<Enemy>().health
                     .CompareTo(a.GetComponent<Enemy>().health));
                break;

            case TargetPriority.LowestHealth:
                targetsInRange.Sort((a, b) =>
                    a.GetComponent<Enemy>().health
                     .CompareTo(b.GetComponent<Enemy>().health));
                break;

            case TargetPriority.MostProgressed:
                // assumes your Enemy has a public float Progress [0..1] or waypointIndex
                targetsInRange.Sort((a, b) =>
                    b.GetComponent<Enemy>().waypointIndex
                     .CompareTo(a.GetComponent<Enemy>().waypointIndex));
                break;

            case TargetPriority.LeastProgressed:
                targetsInRange.Sort((a, b) =>
                    a.GetComponent<Enemy>().waypointIndex
                     .CompareTo(b.GetComponent<Enemy>().waypointIndex));
                break;
        }
    }

    private void FireAtTargets()
    {
        SortTargets();

        int shots = Mathf.Min(targetCount, targetsInRange.Count);
        for (int i = 0; i < shots; i++)
        {
            var e = targetsInRange[i].GetComponent<Enemy>();
            if (e != null) e.TakeDamage(damage);
        }
    }

}
