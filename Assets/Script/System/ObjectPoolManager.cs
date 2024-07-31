using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KeyType = System.String;


[DisallowMultipleComponent]
public class ObjectPoolManager : MonoBehaviour
{
    // ������Ʈ Ǯ ���� �̱���
    public static ObjectPoolManager instance;


    private void Awake()
    {
        SetSingleton();
        InitVariable();
    }


    private void SetSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    private void InitVariable()
    {
        // Dictionary ����
        sampleDict = new Dictionary<KeyType, GameObject>();
        poolDict = new Dictionary<KeyType, Stack<GameObject>>();
        clonePoolDict = new Dictionary<GameObject, Stack<GameObject>>();
    }

  
    // ���� ������Ʈ ����
    private GameObject GetSampleClone(KeyType key)
    {
        if (!sampleDict.TryGetValue(key, out GameObject sample))
        {
            return null;
        }

        return Instantiate(sample);
    }


    public void CreatePool(KeyType key, GameObject prefab, int initialObjectCount, int maxObjectCount)
    {
        // ���� ���ӿ�����Ʈ ����
        GameObject sample = Instantiate(prefab);
        sample.name = key;
        sample.SetActive(false);

        // Pool Dictionary�� Ǯ ���� + Ǯ�� ������Ʈ ����
        Stack<GameObject> pool = new Stack<GameObject>(maxObjectCount);
        for (int i = 0; i < initialObjectCount; i++)
        {
            GameObject clone = Instantiate(prefab);
            clone.SetActive(false);
            pool.Push(clone);

            // Clone Stack ĳ��
            clonePoolDict.Add(clone, pool);
        }

        sampleDict.Add(key, sample);
        poolDict.Add(key, pool);
    }


    // Ǯ���� ��������
    public GameObject Spawn(KeyType key)
    {
        // Ű�� �������� ������ null ��ȯ
        if (!poolDict.TryGetValue(key, out var pool))
        {
            return null;
        }

        GameObject poolObj;

        // Ǯ�� ��� �ִ� ��� : ��������
        if (pool.Count > 0)
        {
            poolObj = pool.Pop();
        }
        // ��� ���� ��� : ������ ����
        else
        {
            poolObj = GetSampleClone(key);

            // Clone Stack�� ĳ��
            clonePoolDict.Add(poolObj, pool); 
        }
        poolObj.SetActive(true);

        return poolObj;
    }


    // Ǯ�� �ٽ� ����ֱ�
    public void Despawn(GameObject poolObj)
    {
        // ĳ�̵� ���ӿ�����Ʈ�� �ƴ� ��� �ı�
        if (!clonePoolDict.TryGetValue(poolObj, out var pool))
        { 
            Destroy(poolObj);
            return;
        }

        // ����ֱ�
        poolObj.SetActive(false);
        pool.Push(poolObj);
    }


    // Member Variable //
    // ������ ������Ʈ�� ���� ��ųʸ�
    private Dictionary<KeyType, GameObject> sampleDict;

    // Ǯ ��ųʸ�
    private Dictionary<KeyType, Stack<GameObject>> poolDict;

    // ������ ���� ������Ʈ Ǯ
    private Dictionary<GameObject, Stack<GameObject>> clonePoolDict;
}
