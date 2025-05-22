using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    public GameObject gun;
    public GameObject sword;
    [SerializeField] private UIManager uIManager;
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        gun.SetActive(false);
        sword.SetActive(true);
    }
    private void Update()
    {
        if(Keyboard.current.qKey.wasPressedThisFrame)
        {
            sword.SetActive(false);
            gun.SetActive(true);
            uIManager.OnGun();
        }
        else if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            gun.SetActive(false);
            sword.SetActive(true);
            uIManager.OnSword();
        }
    }
}
