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
        int length = poolObjectDataList.Count;
        if (length <= 0) { return; }

        // Dictionary 생성
        sampleDict = new Dictionary<KeyType, GameObject>(length);
        dataDict = new Dictionary<KeyType, PoolObjectData>(length);
        poolDict = new Dictionary<KeyType, Stack<GameObject>>(length);
        clonePoolDict = new Dictionary<GameObject, Stack<GameObject>>(length * PoolObjectData.INITIAL_COUNT);

        // Data로 새로운 Pool 오브젝트 정보 생성
        foreach (var data in poolObjectDataList)
        {
            RegisterInternal(data);
        }
    }


    private void RegisterInternal(PoolObjectData data)
    {
        // 중복 키 체크
        if (poolDict.ContainsKey(data.key))
        {
            return;
        }

        // 샘플 게임오브젝트 생성
        GameObject sample = Instantiate(data.prefab);
        sample.name = data.prefab.name;
        sample.SetActive(false);

        // Pool Dictionary에 생성 + 오브젝트 담기
        Stack<GameObject> pool = new Stack<GameObject>(data.maxObjectCount);
        for (int i = 0; i < data.initialObjectCount; i++)
        {
            GameObject clone = Instantiate(data.prefab);
            clone.SetActive(false);
            pool.Push(clone);

            // Clone Stack에 캐싱
            clonePoolDict.Add(clone, pool);
        }

        // Dictionary에 추가
        sampleDict.Add(data.key, sample);
        dataDict.Add(data.key, data);
        poolDict.Add(data.key, pool);
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


    // 인스펙터에서 오브젝트 풀링 대상 추가
    [SerializeField]
    private List<PoolObjectData> poolObjectDataList = new List<PoolObjectData>();

    // 복제될 오브젝트의 원본 딕셔너리
    private Dictionary<KeyType, GameObject> sampleDict;

    // 풀링 정보 딕셔너리
    private Dictionary<KeyType, PoolObjectData> dataDict;

    // 풀 딕셔너리
    private Dictionary<KeyType, Stack<GameObject>> poolDict;

    // 복제된 게임 오브젝트 풀
    private Dictionary<GameObject, Stack<GameObject>> clonePoolDict;
}
