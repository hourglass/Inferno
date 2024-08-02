using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStat : MonoBehaviour
{
    private void Awake()
    {
        InitVariable();
    }


    // 변수 초기화 함수
    private void InitVariable()
    {
        knockBackForce = 1000f;
    }


    private void OnTriggerEnter2D(Collider2D target)
    {
        // 무기 타입에 충돌 시 피해를 주는 함수 실행

        if (target.CompareTag("Enemy"))
        {
            GiveToDamage(target.gameObject);
            KnockBack(target.gameObject);

            // 무기의 관통 타입 체크 후 파괴 실행 
            if (!pierceable)
            {
                Destroy(gameObject);
            }
        }
    }


    // 대미지 처리 함수
    protected void GiveToDamage(GameObject obj)
    {
        if (obj.TryGetComponent(out EnemyStat stat))
        {
            ObjectPoolManager.instance.Spawn(HitEffectKey, obj.transform.position, Quaternion.identity);
            stat.TakeDamage(damage);
        }
    }

    // 넉백 수치 만큼 적을 뒤로 밀어내는 함수
    private void KnockBack(GameObject target)
    {
        if (target.TryGetComponent(out Rigidbody2D rb))
        {
            Vector3 forward = target.transform.position - transform.position;
            forward.Normalize();

            rb.AddForce(forward * knockBackRatio * knockBackForce);
        }
    }


    // Member Variable //
    public static Queue<float> damageQueue = new Queue<float>();

    [SerializeField]
    private string HitEffectKey;
    
    [SerializeField]
    private float damage;
    
    [SerializeField]
    private float knockBackRatio;
    
    [SerializeField]
    private bool pierceable;

    private float knockBackForce;
}
