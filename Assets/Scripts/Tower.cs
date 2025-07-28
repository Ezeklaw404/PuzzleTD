using System;
using System.Collections;
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

    public enum Attribute
    {
        basic,
        slow,
        accelerator,
        uplink
    }

    [SerializeField] private TargetPriority priority;
    [SerializeField] private Attribute attribute;
    [SerializeField] private float range;
    [Tooltip("Shots per minute")]
    [SerializeField] private double fireRate;
    [SerializeField] private double damage;
    [SerializeField] private byte targetCount;
    [SerializeField] private int cost;
    public int Cost => cost;
    [Header("Beam Effect")]
    [SerializeField] private Material beamMaterial;
    [SerializeField] private float beamWidth;
    [SerializeField] private float beamDuration;
    private CircleCollider2D rangeCollider;
    private List<Collider2D> targetsInRange = new();
    private float fireCooldown = 0f;
    private HashSet<Enemy> slowedEnemies = new();
    private float accelRampRate = 0.65f;
    private float accelDecayRate = 2.6f;
    private float minAccelMultiplier = 0.7f;
    private float maxAccelMultiplier = 2f;
    private float currentAccelMultiplier;

    void Start()
    {
        rangeCollider = gameObject.AddComponent<CircleCollider2D>();
        rangeCollider.isTrigger = true;
        rangeCollider.radius = range;
        currentAccelMultiplier = minAccelMultiplier;
    }

    void Update()
    {
        rangeCollider.Overlap(
            new ContactFilter2D().NoFilter(),
            targetsInRange
        );

        targetsInRange.RemoveAll(c => c == null || !c.CompareTag("Enemy"));

        if (attribute == Attribute.accelerator)
        {
            float target = targetsInRange.Count > 0
                ? maxAccelMultiplier
                : minAccelMultiplier;
            float rate = targetsInRange.Count > 0
                ? accelRampRate
                : accelDecayRate;

            currentAccelMultiplier = Mathf.MoveTowards(
                currentAccelMultiplier,
                target,
                rate * Time.deltaTime
            );
        }

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
            var enemyCollider = targetsInRange[i];
            var enemy = enemyCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                switch (attribute)
                {
                    case Attribute.basic:
                        enemy.TakeDamage(damage);
                        break;

                    case Attribute.slow:
                        enemy.TakeDamage(damage);
                        if (!slowedEnemies.Contains(enemy))
                        {
                            enemy.moveSpeed *= 0.7f;
                            slowedEnemies.Add(enemy);
                        }
                        break;

                    case Attribute.accelerator:
                        double finalDamage = damage * currentAccelMultiplier;
                        enemy.TakeDamage(finalDamage);
                        break;

                    case Attribute.uplink:
                        enemy.TakeDamage(damage);
                        enemy.TakeDamage(damage);
                        enemy.TakeDamage(damage);
                        break;
                }

                DrawBeam(transform.position, enemyCollider.transform.position);
            }
        }
    }

    private void DrawBeam(Vector3 from, Vector3 to)
    {
        // 1) create the beam GO
        GameObject go = new GameObject("Beam");
        go.transform.position = Vector3.zero; // or wherever you like

        // 2) add & configure LineRenderer
        var lr = go.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.material = new Material(beamMaterial);
        lr.startWidth = beamWidth;
        lr.endWidth = beamWidth;
        lr.useWorldSpace = true;
        lr.SetPosition(0, from);
        lr.SetPosition(1, to);
        lr.sortingLayerName = "Foreground";

        // 3) add the Beam script to handle fade+destroy
        var beam = go.AddComponent<Beam>();
        beam.duration = beamDuration;
    }
}