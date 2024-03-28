using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Circle : MonoBehaviour
{
    public Transform playerTm;
    public GameObject point;

    private LineRenderer lineRenderer;

    private int vertexCount = 40;
    private float LineWidth = 0.5f;
    private float radius = 7.5f;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        InvokeRepeating("SetUpCircle", 0f, 0.02f);
        InvokeRepeating("PlaceOnCircle", 0f, 0.02f);
    }

    private void SetUpCircle()
    {
        // 라인 선 굵기 설정
        lineRenderer.widthMultiplier = LineWidth;

        float deltaTheta = (2f * Mathf.PI) / vertexCount;
        float theta = 0f;

        // theta 각도 만큼 회전 하면서 라인 그리기
        lineRenderer.positionCount = vertexCount;
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            Vector2 pos = new Vector2(radius * Mathf.Sin(theta), radius * Mathf.Cos(theta));
            lineRenderer.SetPosition(i, pos + (Vector2)playerTm.position);
            theta += deltaTheta;
        }
    }

    private void PlaceOnCircle()
    {
        // 플레이어에서 마우스를 향하는 벡터 얻기                    56      
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 pos = mousePos - (Vector2)playerTm.position;

        // 오브젝트 위치   
        float angle = Mathf.Atan2(pos.x, pos.y) * Mathf.Rad2Deg; // 원점에서 해당 벡터의 사이 각도 구하기
        pos.x = radius * Mathf.Sin(angle * Mathf.Deg2Rad);       // 구한 각도의 sin값은 x의 거리
        pos.y = radius * Mathf.Cos(angle * Mathf.Deg2Rad);       // 구한 각도의 cos값은 y의 거리

        // 현재 플레이어 위치에서 계산한 거리만큼 이동
        point.transform.position = pos + (Vector2)playerTm.position;

        //오브젝트 회전
        float dir = (Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg);

        // 계산 방향에서 스프라이트 방향만큼 회전
        point.transform.rotation = Quaternion.Euler(0f, 0f, dir - 90f); 
    }
}
