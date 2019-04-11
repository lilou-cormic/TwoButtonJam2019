using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : Spawner
{
    private static EnemyDef[] EnemyDefs = null;

    private static Enemy[] EnemyPool;

    private static int[] Positions = new int[] { -2, 0, 2 };

    private void Awake()
    {
        MaxCountDown /= GameOptions.GameSpeed;

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

    protected override void AjustCoolDown()
    {
        MinCountDown = Mathf.Min(MinCountDown - 0.01f * Time.deltaTime, 0.7f);
        MaxCountDown = Mathf.Max(MaxCountDown - 0.005f * Time.deltaTime, 1f);
    }

    protected override void Spawn()
    {
        Enemy enemyPrefab = EnemyPool[Random.Range(0, EnemyPool.Length)];
        int x = Positions[Random.Range(0, Positions.Length)];

        Instantiate(enemyPrefab, transform.position + (Vector3.right * x), Quaternion.identity, transform);
    }
}
