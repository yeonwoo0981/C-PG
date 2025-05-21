using UnityEngine;

public class HHHHHHH : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttack"))
        {
            Destroy(gameObject);
        }
    }
}
