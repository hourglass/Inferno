using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KeyType = System.String;


[DisallowMultipleComponent]
public class ObjectPoolManager : MonoBehaviour
{
    // 오브젝트 풀 관리 싱글톤
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
        // Dictionary 생성
        sampleDict = new Dictionary<KeyType, GameObject>();
        poolDict = new Dictionary<KeyType, Stack<GameObject>>();
        clonePoolDict = new Dictionary<GameObject, Stack<GameObject>>();
    }

  
    // 샘플 오브젝트 복제
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
        // 샘플 게임오브젝트 생성
        GameObject sample = Instantiate(prefab);
        sample.name = key;
        sample.SetActive(false);

        // Pool Dictionary에 풀 생성 + 풀에 오브젝트 생성
        Stack<GameObject> pool = new Stack<GameObject>(maxObjectCount);
        for (int i = 0; i < initialObjectCount; i++)
        {
            GameObject clone = Instantiate(prefab);
            clone.SetActive(false);
            pool.Push(clone);

            // Clone Stack 캐싱
            clonePoolDict.Add(clone, pool);
        }

        sampleDict.Add(key, sample);
        poolDict.Add(key, pool);
    }


    // 풀에서 꺼내오기
    public GameObject Spawn(KeyType key)
    {
        // 키가 존재하지 않으면 null 반환
        if (!poolDict.TryGetValue(key, out var pool))
        {
            return null;
        }

        GameObject poolObj;

        // 풀에 재고가 있는 경우 : 꺼내오기
        if (pool.Count > 0)
        {
            poolObj = pool.Pop();
        }
        // 재고가 없는 경우 : 샘플을 복제
        else
        {
            poolObj = GetSampleClone(key);

            // Clone Stack에 캐싱
            clonePoolDict.Add(poolObj, pool); 
        }
        poolObj.SetActive(true);

        return poolObj;
    }


    // 풀에 다시 집어넣기
    public void Despawn(GameObject poolObj)
    {
        // 캐싱된 게임오브젝트가 아닌 경우 파괴
        if (!clonePoolDict.TryGetValue(poolObj, out var pool))
        { 
            Destroy(poolObj);
            return;
        }

        // 집어넣기
        poolObj.SetActive(false);
        pool.Push(poolObj);
    }


    // Member Variable //
    // 복제될 오브젝트의 원본 딕셔너리
    private Dictionary<KeyType, GameObject> sampleDict;

    // 풀 딕셔너리
    private Dictionary<KeyType, Stack<GameObject>> poolDict;

    // 복제된 게임 오브젝트 풀
    private Dictionary<GameObject, Stack<GameObject>> clonePoolDict;
}
