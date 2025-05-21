using System.Collections;
using UnityEngine;

public class ZonZombieAttack : MonoBehaviour
{
    private float speed = 4f;
    private Vector3 vec;
    private GameObject player;
    [SerializeField] private GameObject zombie;
    private Vector3 aaa;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        vec = player.transform.position - transform.position;
        vec.Normalize();
        aaa = player.transform.position;
        StartCoroutine(Cooltime());
    }

    private IEnumerator Cooltime()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, aaa + (Vector3)new Vector2(0, 0.5f), 2 * Time.deltaTime);
        locate();
        if (Vector3.Distance(transform.position, aaa + new Vector3(0, 0.5f, 0)) < 0.01f)
        {
            Destroy(gameObject);
        }
    }

    void locate()
    {
        if (vec.x > 0)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (vec.x < 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
