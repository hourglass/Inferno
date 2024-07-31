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
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    private void Start()
    {
        Init();
    }


    private void Init()
    {
        int len = poolObjectDataList.Count;
        if (len == 0) return;

        // Dictionary ����
        sampleDict = new Dictionary<KeyType, GameObject>(len);
        dataDict = new Dictionary<KeyType, ObjectPoolData>(len);
        poolDict = new Dictionary<KeyType, Stack<GameObject>>(len);
        clonePoolDict = new Dictionary<GameObject, Stack<GameObject>>(len * ObjectPoolData.INITIAL_COUNT);

        // Data�� ���ο� Pool ������Ʈ ���� ����
        foreach (var data in poolObjectDataList)
        {
            RegisterInternal(data);
        }
    }


    private void RegisterInternal(ObjectPoolData data)
    {
        // �ߺ� Ű Ȯ��
        if (poolDict.ContainsKey(data.key))
        {
            return;
        }

        // ���� ���ӿ�����Ʈ ����
        GameObject sample = Instantiate(data.prefab);
        sample.name = data.prefab.name;
        sample.SetActive(false);

        // Pool Dictionary�� Ǯ ���� + Ǯ�� ������Ʈ ����
        Stack<GameObject> pool = new Stack<GameObject>(data.maxObjectCount);
        for (int i = 0; i < data.initialObjectCount; i++)
        {
            GameObject clone = Instantiate(data.prefab);
            clone.SetActive(false);
            pool.Push(clone);

            // Clone Stack ĳ��
            clonePoolDict.Add(clone, pool); 
        }

        // ��ųʸ��� �߰�
        sampleDict.Add(data.key, sample);
        dataDict.Add(data.key, data);
        poolDict.Add(data.key, pool);
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


    // �ν����Ϳ��� ������Ʈ Ǯ�� ��� �߰�
    [SerializeField]
    private List<ObjectPoolData> poolObjectDataList = new List<ObjectPoolData>();

    // Ǯ�� ���� ��ųʸ�
    private Dictionary<KeyType, ObjectPoolData> dataDict;

    // ������ ������Ʈ�� ���� ��ųʸ�
    private Dictionary<KeyType, GameObject> sampleDict;

    // Ǯ ��ųʸ�
    private Dictionary<KeyType, Stack<GameObject>> poolDict;

    // ������ ���� ������Ʈ Ǯ
    private Dictionary<GameObject, Stack<GameObject>> clonePoolDict;
}
