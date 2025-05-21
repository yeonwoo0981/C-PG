using UnityEngine;
using System.Collections;
using System.Security.Cryptography.X509Certificates;

public class GunOffset : MonoBehaviour
{
    public Material lineMaterial;
    public EnemyHP enemyHP;
    public float coolTime;
    private Vector3 offset = new Vector3(0f, 1f, 0f);  // 목표 1: 항상 기준이 되는 위치 오프셋
    private void Awake()
    {
        coolTime = 1.5f;
    }
    void Update()
    {
        coolTime += Time.deltaTime;
        if (Input.GetMouseButtonDown(0))
        {
            if(coolTime > 1.5f)
            {
                DrawLineToMouse();
                coolTime = 0;
            }
        }
    }

    void DrawLineToMouse()
    {
        Vector3 startPoint = transform.position + offset;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        GameObject lineObj = new GameObject("Line");
        LineRenderer lr = lineObj.AddComponent<LineRenderer>();

        lr.material = lineMaterial;
        lr.positionCount = 2;
        lr.startWidth = 0.03f;  // 초기 너비
        lr.endWidth = 0.03f;
        lr.useWorldSpace = true;

        lr.SetPosition(0, startPoint);
        lr.SetPosition(1, mouseWorldPos);

        StartCoroutine(UpdateLineWidthAfterDelay(lr, 0.075f, 0.1f));

        Destroy(lineObj, 0.2f);
    }

    IEnumerator UpdateLineWidthAfterDelay(LineRenderer lr, float delay, float newWidth)
    {
        yield return new WaitForSeconds(delay);
        if (lr != null)
        {
            lr.startWidth = newWidth;
            lr.endWidth = newWidth;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            enemyHP.MinusHP();
        }
    }
}
