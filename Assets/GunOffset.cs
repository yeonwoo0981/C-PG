using UnityEngine;

public class GunOffset : MonoBehaviour
{
    public Material lineMaterial;

    private Vector3 offset = new Vector3(0f, 1f, 0f);  // 목표 1: 항상 기준이 되는 위치 오프셋

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

        GameObject lineObj = new GameObject("Line");
        LineRenderer lr = lineObj.AddComponent<LineRenderer>();

        lr.material = lineMaterial;
        lr.positionCount = 2;
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.useWorldSpace = true;

        lr.SetPosition(0, startPoint);
        lr.SetPosition(1, mouseWorldPos);

        Destroy(lineObj, 2f);
    }
}
