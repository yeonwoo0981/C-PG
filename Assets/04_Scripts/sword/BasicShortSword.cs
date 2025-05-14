using UnityEngine;

public class BasicShortSword : MonoBehaviour, ISword
{
    public float SwordDamage { get; set ; }

    public void Sword(GameObject sword)
    {
        SwordDamage = .5f;
    }
}
