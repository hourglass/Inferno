using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SlashWave : WeaponStat
{
    [SerializeField] float moveSpeed = 0f;

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * moveSpeed;

        Invoke("SelfDestroy", 0.5f);
    }

    void SelfDestroy()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            GiveToDamage(other.gameObject);
        }
    }
}
