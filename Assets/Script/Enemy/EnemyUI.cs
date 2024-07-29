using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    private void Awake()
    {
        // 체력바 생성 델리게이트
        EnemyManager.CreateHPDel = CreateHp;

        // 체력 감소 델리게이트
        EnemyStat.HpDel = DecreaseHP;        
    }


    private void OnDestroy()
    {
        EnemyManager.CreateHPDel -= CreateHp;
        EnemyStat.HpDel -= DecreaseHP;
    }


    private void Update()
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
    private void CreateHp(Transform enemyTm)
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
    private void DecreaseHP(Transform enemyTm, float health)
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


    // Delegate //
    //최대 체력을 받아올 함수
    public delegate float EnemyStatMaxHeathDel();
    public static EnemyStatMaxHeathDel MaxHeathDel;

    // Member Variable //
    [SerializeField]
    private Canvas enemyCanvas;
    
    [SerializeField]
    private Slider hpBar;

    private List<Transform> enemyList = new List<Transform>();
    private List<Slider> hpList = new List<Slider>();
}
