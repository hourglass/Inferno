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
    private void InitVariable()
    {
        laserAudio = GetComponent<AudioSource>();
        laserAudio.clip = laserSound;

        layerMask = 0;
        delDistance = 2000f;
        hitState = true;
    }


    private void Update()
    {
        ShootLaser();
    }


    private void ShootLaser()
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


    private void DrawRay(Vector2 startPos, Vector2 endPos)
    {
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }


    private IEnumerator Hit(Vector2 pos)
    {
        hitState = false;

        Instantiate(laserHit, pos, Quaternion.identity);

        yield return new WaitForSeconds(attackDelay);

        hitState = true;
    }


    // Member Variable //
    [SerializeField]
    private LineRenderer lineRenderer ;
    
    [SerializeField]
    private Transform laserPoint;
    
    [SerializeField]
    private GameObject laserHit;
    
    [SerializeField]
    private AudioClip laserSound;
    
    [SerializeField]
    private AudioSource laserAudio;

    [SerializeField]
    private float attackDelay;

    private int layerMask;
    private float delDistance;
    private bool hitState;
}
