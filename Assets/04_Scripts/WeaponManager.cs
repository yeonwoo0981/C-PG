using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    public GameObject gun;
    public GameObject sword;
    private void Awake()
    {
        gun.SetActive(false);
        sword.SetActive(true);
    }
    private void Update()
    {
        if(Keyboard.current.qKey.wasPressedThisFrame)
        {
            gun.SetActive(true);
            sword.SetActive(false);
        }
        else if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            gun.SetActive(false);
            sword.SetActive(true);
        }
    }
}
