using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(DestroyInLifeTime());
    }


    private void OnDisable()
    {
        StopAllCoroutines();
    }


    IEnumerator DestroyInLifeTime()
    {
        WaitForSeconds delay = new WaitForSeconds(lifeTime);
        yield return delay;

        if (gameObject.TryGetComponent(out ObjectPoolData data))
        {
            ObjectPoolManager.instance.Despawn(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    //Member Variable//
    [SerializeField]
    private float lifeTime = 0;
}
