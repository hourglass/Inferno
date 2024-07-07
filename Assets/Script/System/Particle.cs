using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }


    void Update()
    {
        if (ps != null)
        {
            ParticleSystem.MainModule main = ps.main;

            if (main.startRotation.mode == ParticleSystemCurveMode.Constant)
            {
                main.startRotation = -transform.eulerAngles.z * Mathf.Deg2Rad;
            }
        }
    }


    //Member Variable//
    [SerializeField] ParticleSystem ps = null;
}
