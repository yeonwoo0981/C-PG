using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "SO/Sword")]
public class WeaPon : ScriptableObject
{
    public float Damage;
    public string WeaponName;
    public Sprite WeaponAsset;
}
