using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer = null;
    [SerializeField] Transform laserPoint = null;
    [SerializeField] GameObject laserHit = null;

    [SerializeField] float attackDelay = 0.25f;

    //오디오
    [SerializeField] AudioClip laserSound = null;
    [SerializeField] AudioSource laserAudio = null;

    float delDistance = 2000f;
    bool hitState = true;
    int layerMask = 0;

    Transform tm;

    private void Start()
    {
        TowerManager.LaserDel += SetAttackDelay;
        
        tm = GetComponent<Transform>();
        laserAudio = GetComponent<AudioSource>();

        laserAudio.clip = laserSound;
        laserAudio.Play();

        InvokeRepeating("ShootLaser", 0f, 0.02f);
    }

    void ShootLaser()
    {
        layerMask = 1 << LayerMask.NameToLayer("Enemy");

        if (Physics2D.Raycast(tm.position, transform.right, delDistance, layerMask))
        {
            RaycastHit2D hit = Physics2D.Raycast(tm.position, transform.right, delDistance, layerMask);
            DrawRay(laserPoint.position, hit.point);

            if (hit.transform.tag == "Enemy" && hitState)
            {
                StartCoroutine(Hit(hit.point));
            }
        }
        else
        {
            DrawRay(laserPoint.position, laserPoint.position);
        }
    }

    void DrawRay(Vector2 startPos, Vector2 endPos)
    {
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }

    void SetAttackDelay()
    {
        attackDelay = 0.15f;
    }

    IEnumerator Hit(Vector2 pos)
    {
        hitState = false;

        Instantiate(laserHit, pos, Quaternion.identity);

        yield return new WaitForSeconds(attackDelay);

        hitState = true;
    }
}
