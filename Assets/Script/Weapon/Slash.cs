using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    void Awake()
    {
        Transform playerTm = GameObject.Find("PlayerManager").transform;
        transform.SetParent(playerTm);
    }
}
