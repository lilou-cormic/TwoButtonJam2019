using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy")]
public class EnemyDef : ScriptableObject
{
    [SerializeField]
    private Enemy _EnemyPrefab = null;
    public Enemy EnemyPrefab { get => _EnemyPrefab; }

    [SerializeField]
    private float _SpawnWeight = 1f;
    public float SpawnWeight { get => _SpawnWeight; }
}
