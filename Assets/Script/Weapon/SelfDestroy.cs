using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }


    //Member Variable//
    [SerializeField] float lifeTime = 0;
}
