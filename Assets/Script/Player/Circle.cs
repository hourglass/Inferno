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
        lineRenderer.widthMultiplier = LineWidth;

        float deltaTheta = (2f * Mathf.PI) / vertexCount;
        float theta = 0f;

        lineRenderer.positionCount = vertexCount;
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            Vector2 pos = new Vector2(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta));
            lineRenderer.SetPosition(i, pos + (Vector2)playerTm.position);
            theta += deltaTheta;
        }
    }

    private void PlaceOnCircle()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 pos = mousePos - (Vector2)playerTm.position;

        //오브젝트 위치
        float angle = Mathf.Atan2(pos.x, pos.y) * Mathf.Rad2Deg;
        pos.x = radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        pos.y = radius * Mathf.Cos(angle * Mathf.Deg2Rad);

        point.transform.position = pos + (Vector2)playerTm.position;

        //오브젝트 회전
        float dir_x = mousePos.x - transform.position.x;
        float dir_y = mousePos.y - transform.position.y;
        float dir = (Mathf.Atan2(dir_y, dir_x) * Mathf.Rad2Deg);

        point.transform.rotation = Quaternion.Euler(0f, 0f, dir - 90f);
    }
}
