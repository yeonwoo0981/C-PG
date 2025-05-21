using UnityEngine;
using UnityEngine.InputSystem;

public class SwordDamage : MonoBehaviour
{
    Rigidbody2D rb;
    private Vector2 moveDir;
    public Transform player;
    public Vector3 offset = new Vector3(0, 1, 0);

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if(moveDir.x > 0)
        {
            transform.position = player.position + offset;
        }

        else if(moveDir.x < 0)
        {
            transform.position = player.position - offset;
        }
    }
    void Update()
    {
        Debug.DrawRay(rb.position, Vector2.right.normalized, Color.red, 5.5f);
        RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.right.normalized, 5.5f, LayerMask.GetMask("Enemy"));
    }
}
