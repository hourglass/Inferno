using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private void Awake()
    {
        InitVariable();
        laserAudio.Play();
    }


    // 변수 초기화 함수
    void InitVariable()
    {
        laserAudio = GetComponent<AudioSource>();
        laserAudio.clip = laserSound;

        layerMask = 0;
        delDistance = 2000f;
        hitState = true;
    }


    void Update()
    {
        ShootLaser();
    }


    void ShootLaser()
    {
        layerMask = 1 << LayerMask.NameToLayer("Enemy");

        if (Physics2D.Raycast(transform.position, transform.right, delDistance, layerMask))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, delDistance, layerMask);
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


    IEnumerator Hit(Vector2 pos)
    {
        hitState = false;

        Instantiate(laserHit, pos, Quaternion.identity);

        yield return new WaitForSeconds(attackDelay);

        hitState = true;
    }


    //Member Variable//
    [SerializeField] LineRenderer lineRenderer ;
    [SerializeField] Transform laserPoint;
    [SerializeField] GameObject laserHit;
    [SerializeField] AudioClip laserSound;
    [SerializeField] AudioSource laserAudio;

    [SerializeField] float attackDelay;

    int layerMask;
    float delDistance;
    bool hitState;
}
