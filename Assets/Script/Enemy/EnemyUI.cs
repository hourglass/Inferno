using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    //최대 체력을 받아올 함수
    public delegate float EnemyStatMaxHeathDel();
    public static EnemyStatMaxHeathDel MaxHeathDel;

    [SerializeField] Canvas enemyCanvas = null;
    [SerializeField] Slider hpBar = null;

    List<Transform> enemyList = new List<Transform>();
    List<Slider> hpList = new List<Slider>();


    void Start()
    {
        // 델리게이트 등록 함수
        EnemyManager.CreateHPDel = CreateHp; // Hp바 생성 함수
        EnemyStat.HpDel = DecreaseHP;        // Hp감소 함수
    }

    void Update()
    {
        Camera cam = Camera.main;

        // Enemy List를 순회하면서 Hp바 위치를 갱신
        for (int i = 0; i < hpList.Count; i++)
        {
            if (hpList[i] != null && enemyList[i] != null)
            {
                hpList[i].transform.position
                    = cam.WorldToScreenPoint(enemyList[i].transform.position + new Vector3(0f, -4f, 0f));
            }
        }
    }

    // Hp바 생성 함수
    void CreateHp(Transform enemyTm)
    {
        Slider hp = Instantiate(hpBar) as Slider;
        hp.transform.SetParent(enemyCanvas.transform);
        hp.minValue = 0;
        hp.maxValue = MaxHeathDel();
        hp.value = MaxHeathDel();

        hpList.Add(hp);
        enemyList.Add(enemyTm);
    }

    // Hp바 갱신 함수
    void DecreaseHP(Transform enemyTm, float health)
    {
        int index = enemyList.IndexOf(enemyTm);

        hpList[index].value = health;

        if (hpList[index].value <= 0)
        {
            Destroy(hpList[index].gameObject);
            hpList.RemoveAt(index);
            enemyList.RemoveAt(index);
        }
    }
}
