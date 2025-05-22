using System.Collections;
using UnityEngine;

public class AAB : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(Cooltime());
    }

    private IEnumerator Cooltime()
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }
}
