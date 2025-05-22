using UnityEngine;
using System.Collections;

public class GunOffset : MonoBehaviour
{
    public Material lineMaterial;
    public EnemyHP enemyHP;
    private Vector3 offset = new Vector3(0f, 0.4f, 0f);
    public Transform player;
    [SerializeField]private float coolTime;
    [SerializeField] Rigidbody2D rb;
    public Vector2 moveDir;
    public GameObject sword;
    public float rayLength = 10f;
    private Animator animator;
    public PlayerM playerM;
    private void Awake()
    {
        animator = playerM.GetComponent<Animator>();
    }
    void FixedUpdate()
    {
        coolTime += Time.deltaTime;
        if (coolTime >= 1.5f)
        {
            if (Input.GetMouseButtonDown(0))
            {
                sword.SetActive(false);
                DrawLineToMouse();
                coolTime = 0;
            }
        }
        transform.position = player.transform.position + offset;
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
        lr.startWidth = 0.03f;
        lr.endWidth = 0.03f;
        lr.useWorldSpace = true;

        lr.SetPosition(0, startPoint);
        lr.SetPosition(1, mouseWorldPos);

        Destroy(lineObj, 2f);
        StartCoroutine(UpdateLineWidthAfterDelay(lr, 0.075f, 0.1f));

        Destroy(lineObj, 0.2f);
    }

    IEnumerator UpdateLineWidthAfterDelay(LineRenderer lr, float delay, float newWidth)
    {

        float moveSpeed = PlayerManager.Instance != null ? PlayerManager.Instance.moveSpeed : 0f;
        animator.SetBool("isGun", true);
        yield return new WaitForSeconds(delay);
        if (lr != null)
        {
            lr.startWidth = newWidth;
            lr.endWidth = newWidth;
        }
        animator.SetBool("isGun", false);
        moveSpeed = PlayerManager.Instance != null ? PlayerManager.Instance.moveSpeed : 5f;
    }
}

