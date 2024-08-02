using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KeyType = System.String;

[System.Serializable]
public class ObjectPoolData : MonoBehaviour
{
    // Member Variable //
    public const int INITIAL_COUNT = 10;
    public const int MAX_COUNT = 50;

    public KeyType key;
    public int initialObjectCount = INITIAL_COUNT;
    public int maxObjectCount = MAX_COUNT;
}
