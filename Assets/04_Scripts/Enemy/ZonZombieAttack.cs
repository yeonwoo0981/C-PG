using UnityEngine;

public class ZonZombieAttack : MonoBehaviour
{
    private float speed = 2f;
    private Vector3 vec;
    private GameObject player;
    [SerializeField] private GameObject zombie;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        vec = player.transform.position - transform.position;
        vec.Normalize();
        Instantiate(this, zombie.transform.position, Quaternion.identity);
    }

    private void Update()
    {
        transform.position += vec * speed * Time.deltaTime;
        locate();
    }

    void locate()
    {
        if (vec.x > 0)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (vec.x < 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }
}
