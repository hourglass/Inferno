using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ArrowHoming : WeaponStat
{
    [SerializeField] float moveSpeed = 0f;

    [SerializeField] LayerMask enemyMask = 0;
    Rigidbody2D rb;
    Transform target = null;

    float range = 500f;
    float rotSpeed = 1000f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("Homing", 0f, 0.05f);
    }

    void Homing()
    {
        if (target == null)
        {
            rb.velocity = transform.up * moveSpeed;
            SearchTarget();
        }
        else
        {
            Vector2 direction = (Vector2)target.position - rb.position;
            direction.Normalize();

            float rotAmount = Vector3.Cross(direction, transform.up).z;

            rb.angularVelocity = -rotAmount * rotSpeed;
            rb.velocity = transform.up * moveSpeed;
        }
    }

    void SearchTarget()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, range, enemyMask);

        if (cols.Length > 0)
        {
            target = cols[Random.Range(0, cols.Length)].transform;
        }
    }
}
