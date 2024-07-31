using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolRegister : MonoBehaviour
{
    void Start()
    {
        Register();
    }


    private void Register()
    {
        if (prefabs.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < prefabs.Count; ++i)
        {
            if (prefabs[i].TryGetComponent(out ObjectPoolData poolObj))
            {
                ObjectPoolManager.instance?.CreatePool(
                    poolObj.key,
                    prefabs[i],
                     poolObj.initialObjectCount,
                     poolObj.maxObjectCount
                    );
            }
        }
    }


    // Member Variable //
    [SerializeField]
    private List<GameObject> prefabs = new List<GameObject>();
}
