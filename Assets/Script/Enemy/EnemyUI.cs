using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    private void Awake()
    {
        // 체력바 생성 델리게이트
        EnemySpawner.CreateHPDel = CreateHp;

        // 체력 감소 델리게이트
        EnemyStat.DecreaseHpDel = DecreaseHP;
    }


    private void OnDestroy()
    {
        //EnemyManager.CreateHPDel -= CreateHp;
        //EnemyStat.HpDel -= DecreaseHP;
    }


    private void Update()
    {
        Camera cam = Camera.main;

        // hpBarDict를 순회하면서 Hp바 위치를 갱신
        foreach (KeyValuePair<GameObject,Slider> item in hpBarDict)
        {
            if (item.Key != null && item.Value != null)
            {
                item.Value.transform.position = cam.WorldToScreenPoint(item.Key.transform.position + new Vector3(0f, -4f, 0f));
            }
        }
    }


    // Hp바 생성 함수
    private void CreateHp(GameObject enemy)
    {
        if(enemy == null) { return; }

        GameObject hpObj = ObjectPoolManager.instance.Spawn("Enemy_HpBar");
        if (hpObj.TryGetComponent(out Slider hp))
        {
            hp.transform.SetParent(enemyCanvas.transform);
            hp.minValue = 0;
            hp.maxValue = MaxHeathDel();
            hp.value = hp.maxValue;

            if (!hpBarDict.ContainsKey(enemy))
            {
                hpBarDict.Add(enemy, hp);
            }
        }
    }


    // Hp바 갱신 함수
    private void DecreaseHP(GameObject enemy, float health)
    {
        if (!hpBarDict.ContainsKey(enemy))
        {
            return;
        }

        lock (this)
        {
            hpBarDict[enemy].value = health;
            if (hpBarDict[enemy].value <= 0)
            {
                ObjectPoolManager.instance.Despawn(hpBarDict[enemy].gameObject);
                hpBarDict.Remove(enemy);
            }
        }
    }


    // Delegate //
    //최대 체력을 받아올 함수
    public delegate float EnemyStatMaxHeathDel();
    public static EnemyStatMaxHeathDel MaxHeathDel;

    // Member Variable //
    [SerializeField]
    private Canvas enemyCanvas;

    private Dictionary<GameObject, Slider> hpBarDict =  new Dictionary<GameObject, Slider>();
}
