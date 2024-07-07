using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStat : MonoBehaviour
{
    void Awake()
    {
        InitVariable();
    }


    // 변수 초기화 함수
    void InitVariable()
    {
        knockBackForce = 1000f;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        // 무기 타입에 충돌 시 피해를 주는 함수 실행
        if (other.tag == "Enemy")
        {
            if (other.gameObject != null)
            {
                GiveToDamage(other.gameObject);
                KnockBack(other.gameObject);
            }

            // 무기의 관통 타입 체크 후 파괴 실행 
            if (!pierceable)
            {
                Destroy(gameObject);
            }
        }
    }


    // 대미지 큐에 피해량을 저장하는 함수
    protected void GiveToDamage(GameObject obj)
    {
        damageQueue.Enqueue(damage);
        if (weaponHit != null)
        {
            Instantiate(weaponHit, obj.transform.position, Quaternion.identity);
        }
    }

    // 넉백 수치 만큼 적을 뒤로 밀어내는 함수
    void KnockBack(GameObject target)
    {
        var rigidbody = target.GetComponent<Rigidbody2D>();
        if (rigidbody != null)
        {
            var forward = target.transform.position - transform.position;
            forward.Normalize();

            rigidbody.AddForce(forward * knockBackRatio * knockBackForce);
        }
    }


    //Member Variable//
    public static Queue<float> damageQueue = new Queue<float>();

    [SerializeField] GameObject weaponHit;
    [SerializeField] float damage;
    [SerializeField] float knockBackRatio;
    [SerializeField] bool pierceable;

    float knockBackForce;
}
