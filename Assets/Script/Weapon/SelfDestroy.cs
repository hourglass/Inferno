using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }


    //Member Variable//
    [SerializeField]
    private float lifeTime = 0;
}
