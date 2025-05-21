using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gunImage;
    [SerializeField] private GameObject swordImage;
    private void Awake()
    {
        gunImage.SetActive(true);
        swordImage.SetActive(false);
    }
    public void OnGun()
    {
        gunImage.SetActive(true);
        swordImage.SetActive(false);
    }
    public void OnSword()
    {
        gunImage.SetActive(false);
        swordImage.SetActive(true);
    }
}
