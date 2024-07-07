using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CircleUI : MonoBehaviour
{
    void Awake()
    {
        InitVariable();
    }


    // 변수 초기화 함수
    void InitVariable()
    {
        // 멤버 변수 초기화
        lineRenderer = GetComponent<LineRenderer>();
        drawCount = 40;
        LineWidth = 0.5f;
        radius = 7.5f;
    }


    void Update()
    {
        if (!ChoiceManager.gameIsPaused)
        {
            DrawCircle();
            PlaceOnCircle();
        }
    }


    void DrawCircle()
    {
        // 라인 선 굵기 설정
        lineRenderer.widthMultiplier = LineWidth;

        float deltaTheta = (2f * Mathf.PI) / drawCount;
        float theta = 0f;

        // theta 각도 만큼 회전 하면서 라인 그리기
        lineRenderer.positionCount = drawCount;
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            Vector2 pos = new Vector2(radius * Mathf.Sin(theta), radius * Mathf.Cos(theta));
            lineRenderer.SetPosition(i, pos + (Vector2)playerTm.position);
            theta += deltaTheta;
        }
    }


    void PlaceOnCircle()
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


    //Member Variable//
    public Transform playerTm;
    public GameObject point;

    LineRenderer lineRenderer;

    int drawCount;
    float LineWidth;
    float radius;
}
