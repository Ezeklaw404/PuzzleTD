using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private float fireRate;
    [SerializeField] private float damage;
    [SerializeField] private byte targetCount;
    [SerializeField] private CircleCollider2D rangeCollider;
    private List<Collider2D> targetsInRange = new();
    private Queue<Enemy> targetsToShoot = new();
    void Start()
    {
        rangeCollider.radius = range;
        rangeCollider.enabled = true;
    }
    void Update()
    {
        rangeCollider.Overlap(new ContactFilter2D().NoFilter(), targetsInRange);
        if (targetsInRange.Count > 0)
        {

        }
    }
}