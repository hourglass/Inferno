using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] float baseValue = 0f;

    public float getValue()
    {
        return baseValue;
    }
}
