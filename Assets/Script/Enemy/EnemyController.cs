using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : EnemyStat
{
    [SerializeField] GameObject afterImageEffect = null;

    GameObject player = null;

    Rigidbody2D rb;
    Vector2 forward;
    Vector2 playerPos;

    float rotSpeed = 500f;
    float moveSpeed = 10f;
    float dashSpeed = 40f;

    float attackDuration = 2f;
    float attackDelay = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        player = GameObject.FindWithTag("Player");
        InvokeRepeating("ChasePlayer", 0f, 0.2f);
        StartCoroutine(Attack());

        afterImageEffect.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GiveToDamage(other.gameObject);
        }

        if (other.tag == "Weapon")
        {
            var weapon = other.gameObject.GetComponent<WeaponStat>();
            if (weapon != null) 
            {
                KnockBack(weapon.KnockBackForce);
            }

            TakeAllDamage();
        }
    }

    public void SetPlayer(GameObject _player)
    {
        if (player != null)
        {
            player = _player;
        }
    }

    void KnockBack(float power)
    {
        rb.AddForce(-forward * power);
    }

    void ChasePlayer()
    {
        if (player != null)
        {
            playerPos = player.transform.position;

            if (Vector2.Distance(playerPos, rb.position) > 0.2f)
            {
                forward = playerPos - rb.position;
                forward.Normalize();

                float rotAmount = Vector3.Cross(forward, transform.up).z;

                rb.angularVelocity = -rotAmount * rotSpeed;
                rb.velocity = transform.up * moveSpeed;
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
        else
        {
            CancelInvoke("ChasePlayer");
        }
    }

    IEnumerator Attack()
    {
        WaitForSeconds duration = new WaitForSeconds(attackDuration);
        WaitForSeconds delay = new WaitForSeconds(attackDelay);

        while (true)
        {
            yield return delay;

            afterImageEffect.SetActive(true);
            moveSpeed += dashSpeed;

            yield return duration;

            afterImageEffect.SetActive(false);
            moveSpeed -= dashSpeed;
        }
    }
}
