using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HomingObject : MonoBehaviour
{
    void Awake()
    {
        InitVariable();
    }


    // 변수 초기화 함수
    void InitVariable()
    {
        rb = GetComponent<Rigidbody2D>();

        range = 500f;
        rotSpeed = 1000f;
    }


    void FixedUpdate()
    {
        Homing();
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


    //Member Variable//
    [SerializeField] float moveSpeed;

    [SerializeField] LayerMask enemyMask;
    Rigidbody2D rb;
    Transform target;

    float range;
    float rotSpeed;
}
