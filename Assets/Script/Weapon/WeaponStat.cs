using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStat : MonoBehaviour
{
    public static Queue<float> damageQueue = new Queue<float>();

    [SerializeField] GameObject weaponHit = null;
    [SerializeField] float damage = 0f;
    [SerializeField] float knockBackForce = 0f;
    [SerializeField] bool pierceable = false;

    public float Damage { get { return damage; } }
    public float KnockBackForce { get { return knockBackForce; } }

    protected void GiveToDamage(GameObject obj)
    {
        damageQueue.Enqueue(Damage);
        Instantiate(weaponHit, obj.transform.position, Quaternion.identity);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            GiveToDamage(other.gameObject);

            if (!pierceable)
            {
                Destroy(gameObject);
            }
        }
    }
}
