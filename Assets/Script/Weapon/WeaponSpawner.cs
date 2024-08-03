using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        InitVariable();
    }

    // 변수 초기화 함수
    private void InitVariable()
    {
        // 어떤 교단의 인덱스가 선택됐는지 가져오는 함수
        weaponId = GetweaponIdDel();

        // 선택된 인덱스에 해당하는 무기 생성
        GameObject weapon = Instantiate(WeaponList[weaponId], transform.position, Quaternion.identity);

        if (!player.TryGetComponent(out PlayerInput input))
        {
            return;    
        }

        if (!weapon.TryGetComponent(out Weapon weaponObj))
        {
            return;
        }

        input.SetCurrentWeapon(weaponObj);        
    }


    // Delegate //
    // 무기 선택 창에서 선택된 인덱스를 받아올 델리게이트
    public delegate int SceneSelectGetIndexDel();
    public static SceneSelectGetIndexDel GetweaponIdDel;


    // Member Variable //
    // 무기 프리팹
    [SerializeField]
    private List<GameObject> WeaponList = new();

    private GameObject player;

    private int weaponId = 0;
}
