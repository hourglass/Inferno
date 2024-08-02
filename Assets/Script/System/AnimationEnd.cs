using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationEnd : MonoBehaviour
{
    private void Update()
    {
        Animator animator = GetComponent<Animator>();

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            if (gameObject.TryGetComponent(out ObjectPoolData data))
            {
                ObjectPoolManager.instance.Despawn(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }            
        }
    }
}