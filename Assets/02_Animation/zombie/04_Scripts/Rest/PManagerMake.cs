using UnityEngine;

public class PManagerMake : MonoBehaviour
{
    [SerializeField] private GameObject playerManagerPrefab;

    private void Awake()
    {
        
        if (PlayerManager.Instance == null && playerManagerPrefab != null)
        {
            
            Instantiate(playerManagerPrefab);
            Debug.Log("PlayerManager ÀÚµ¿ »ý¼ºµÊ");
        }
    }
}
