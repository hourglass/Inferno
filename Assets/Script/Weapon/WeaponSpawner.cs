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

    // ���� �ʱ�ȭ �Լ�
    private void InitVariable()
    {
        // � ������ �ε����� ���õƴ��� �������� �Լ�
        weaponId = GetweaponIdDel();

        // ���õ� �ε����� �ش��ϴ� ���� ����
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
    // ���� ���� â���� ���õ� �ε����� �޾ƿ� ��������Ʈ
    public delegate int SceneSelectGetIndexDel();
    public static SceneSelectGetIndexDel GetweaponIdDel;


    // Member Variable //
    // ���� ������
    [SerializeField]
    private List<GameObject> WeaponList = new();

    private GameObject player;

    private int weaponId = 0;
}
