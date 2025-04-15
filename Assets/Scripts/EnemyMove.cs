using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigid;
    private float speed = 2;
    private Vector2 vec;
    private GameObject player;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        vec = player.transform.position - transform.position;
    }
    public void FixedUpdate()
    {
        rigid.linearVelocity = vec.normalized * speed;
    }
}
