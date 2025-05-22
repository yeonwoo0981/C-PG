using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class Sword : MonoBehaviour
{
    public GameObject sword;
    public GameObject gun;
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        sword.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !gun.activeSelf)
        {
            sword.SetActive(true);
            animator.SetBool("isSword", true);
            animator.SetBool("isGun", false);
            StartCoroutine(SwordTrue());
        }
    }

    private IEnumerator SwordTrue()
    {
        yield return new WaitForSeconds(1f);
        sword.SetActive(false);
        animator.SetBool("isSword",false);
    }
}
