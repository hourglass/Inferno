using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : PlayerStat
{
    [SerializeField] GameObject particle = null;

    public delegate void WeaponManagerInputKeyDel();
    public static WeaponManagerInputKeyDel InputKeyDel;

    Rigidbody2D rb;
    Vector2 forward;

    float moveSpeed = 25f;
    float stopDistance = 0.5f;

    bool dashState = true;
    float dashSpeed = 40f;
    float dashTime = 0.25f;
    float dashDelay = 0.25f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        particle.SetActive(false);
    }

    void Update()
    {
        if (!ChoiceManager.gameIsPaused)
        {
            InputKeyDel();

            //대쉬
            if (Input.GetMouseButtonDown(1))
            {
                if (dashState)
                {
                    StartCoroutine(DashRoutine());
                }
            }
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = transform.position;

        forward = mousePos - playerPos;
        forward.Normalize();

        if (Vector2.Distance(mousePos, playerPos) > stopDistance)
        {
            rb.velocity = forward * (moveSpeed);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    IEnumerator DashRoutine()
    {
        moveSpeed += dashSpeed;
        particle.SetActive(true);

        yield return new WaitForSeconds(dashTime);

        moveSpeed -= dashSpeed;
        dashState = false;

        yield return new WaitForSeconds(dashDelay);

        dashState = true;
        particle.SetActive(false);
    }
}
