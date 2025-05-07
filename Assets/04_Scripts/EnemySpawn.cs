using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject prefabs;
    private Vector2 spawnx = new Vector2(5f, 23f);

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            float randomx = Random.Range(spawnx.x, spawnx.y);
            Vector2 spawn = new Vector2(randomx, -3.26f);
            GameObject instant = Instantiate(prefabs, spawn, Quaternion.identity); //오브젝트,스폰위치,회전 없음
        }
    }
}
