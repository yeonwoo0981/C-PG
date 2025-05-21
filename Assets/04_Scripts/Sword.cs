using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class Sword : MonoBehaviour
{
    public GameObject sword;
    private void Awake()
    {
        sword.SetActive(false);
    }
    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            sword.SetActive(true);
            StartCoroutine(SwordTrue());
        }
    }
    private IEnumerator SwordTrue()
    {
        yield return new WaitForSeconds(1f);
        sword.SetActive(false);
    }
}
