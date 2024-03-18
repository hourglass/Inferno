using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationEnd : MonoBehaviour
{
    void Start()
    {
        InvokeRepeating("SelfDestroy", 0f, 0.1f);
    }

    void SelfDestroy()
    {
        Animator animator = GetComponent<Animator>();

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Destroy(gameObject);
        }
    }
}
