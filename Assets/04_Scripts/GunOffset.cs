using UnityEngine;
using System.Collections;
using System.Security.Cryptography.X509Certificates;

public class GunOffset : MonoBehaviour
{
    public Material lineMaterial;
    public EnemyHP enemyHP;
    public float coolTime;
    private Vector3 offset = new Vector3(0f, 1f, 0f);

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DrawLineToMouse();
        }
    }

    void DrawLineToMouse()
    {
        Vector3 startPoint = transform.position + offset;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        // 라인 렌더러 생성
        GameObject lineObj = new GameObject("Line");
        LineRenderer lr = lineObj.AddComponent<LineRenderer>();

        lr.material = lineMaterial;
        lr.positionCount = 2;
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
        lr.useWorldSpace = true;

        lr.SetPosition(0, startPoint);
        lr.SetPosition(1, mouseWorldPos);

        // 너비 변경
        StartCoroutine(bbbb(lr, 0.2f, 0.1f));
        IEnumerator bbbb(LineRenderer lr, float delay, float newWidth)
        {
            yield return new WaitForSeconds(delay);
            if (lr != null)
            {
                lr.startWidth = newWidth;
                lr.endWidth = newWidth;
            }
        }
        Destroy(lineObj, 2f);

        // 충돌 감지 (2D Raycast)
        RaycastHit2D hit = Physics2D.Linecast(startPoint, mouseWorldPos);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.name == "Enemy") // 또는 태그로도 가능
            {
                enemyHP.MinusHP();
            }
        }
    }
}
