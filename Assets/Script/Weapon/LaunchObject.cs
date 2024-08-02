using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchObject : MonoBehaviour
{
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void OnEnable()
    {
        rb.velocity = transform.up * moveSpeed;
    }

     
    //Member Variable//
    [SerializeField]
    private float moveSpeed;

    private Rigidbody2D rb;
}
