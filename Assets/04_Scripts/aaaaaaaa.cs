using UnityEngine;

public class aaaaaaaa : MonoBehaviour
{
    [SerializeField] private GameObject dadad;
    [SerializeField] private GameObject dadaaaad;

    private void Awake()
    {
        dadad.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            dadad.SetActive(false);
            dadaaaad.SetActive(true);
        }
    }
}
