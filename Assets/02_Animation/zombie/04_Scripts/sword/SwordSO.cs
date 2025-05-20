using UnityEngine;

public class SwordSO : MonoBehaviour
{
    public WeaPon sword = new WeaPon();
    public float DAMAGE;
    private void Update()
    {
        gameObject.name = sword.name;
        GetComponent<SpriteRenderer>().sprite = sword.WeaponAsset;
        DAMAGE = sword.Damage;
    }
}
