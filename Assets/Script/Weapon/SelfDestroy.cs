using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField] float lifeTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroySelf", lifeTime);
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
