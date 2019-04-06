using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private static EnemyDef[] EnemyDefs = null;

    private static Enemy[] EnemyPool;

    private static int[] Positions = new int[] { -2, 0, 2 };

    private float _countDown = 1.5f;

    private void Awake()
    {
        _countDown = Random.Range(1.5f, 3f);

        if (EnemyDefs == null)
            EnemyDefs = Resources.LoadAll<EnemyDef>("Enemies").ToArray();

        if (EnemyPool == null)
        {
            List<Enemy> pool = new List<Enemy>();

            foreach (EnemyDef enemyDef in EnemyDefs)
            {
                for (int i = 0; i < enemyDef.SpawnWeight; i++)
                {
                    pool.Add(enemyDef.EnemyPrefab);
                }
            }

            EnemyPool = pool.ToArray();
        }
    }

    private void FixedUpdate()
    {
        _countDown -= Time.fixedDeltaTime;

        if (_countDown <= 0)
        {
            _countDown = Random.Range(3f, 5f);

            Spawn();
        }
    }

    private void Spawn()
    {
        Enemy enemyPrefab = EnemyPool[Random.Range(0, EnemyPool.Length)];
        int x = Positions[Random.Range(0, Positions.Length)];

        Instantiate(enemyPrefab, transform.position + (Vector3.right * x), Quaternion.identity, transform);
    }
}
