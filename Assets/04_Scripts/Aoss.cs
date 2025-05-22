using System.Collections;
using UnityEngine;

public class Aoss : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(Cooltime());
    }

    private IEnumerator Cooltime()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
