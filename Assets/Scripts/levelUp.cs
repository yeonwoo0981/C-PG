using UnityEngine;
using TMPro;
using System.Collections;

public class levelUp : MonoBehaviour
{
    public int level = 1;
    public int p_Ex = 0;
    public Hp playerHp;
    [SerializeField] private TextMeshProUGUI _lvUpText;

    private void Start()
    {
        if (_lvUpText != null)
        {
            _lvUpText.gameObject.SetActive(false);
        }
    }
    public void ExUp()
    {
        p_Ex += 55;
        Debug.Log($"경험치 : {p_Ex}");

        if (p_Ex >= 100)
        {
            p_Ex -= 100;
            P_level_Up();
        }
    }

    public void P_level_Up()
    {
        level += 1;
        Debug.Log($"플레이어 레벨: {level}");
        HpPlus();
    }

    private IEnumerator ShowLevelUpText()
    {
        if (_lvUpText != null)
        {

            _lvUpText.gameObject.SetActive(true);
            _lvUpText.text = $"LEVEL UP!";

            yield return new WaitForSeconds(3f);

            _lvUpText.gameObject.SetActive(false);
        }
    }


    public void HpPlus()
    {
        if (playerHp != null)
        {
            playerHp.maxHealth += 10;
            playerHp.curHealth += 10;
            playerHp.UpdateHpText();
        }
    }
}