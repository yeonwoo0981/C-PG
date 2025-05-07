using UnityEngine;
using UnityEngine.Rendering;

public class Test_P_Level : MonoBehaviour
{
    [SerializeField] Test_E_HP enemyHp;
    private int level = 1;
    private int p_Ex = 0;
    private void Update()
    {
        if(enemyHp.Hp < 0)
        {
            enemyHp.E_Dead();
        }
    }

    public void ExUp()
    {
        if(p_Ex < 100)
        {
            p_Ex += 55;
        }
        else
        {
            p_Ex -= 100;
            Debug.Log($"p_Ex : {p_Ex}");
            P_level_Up();
        }
    }

    public void P_level_Up()
    {
        level += 1;
        Debug.Log($"level : {level}");
    }
}
