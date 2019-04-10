using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : Spawner
{
    [SerializeField]
    private Item[] PickupPrefabs = null;

    private void Start()
    {
        MinCountDown = 10f;
        MaxCountDown = 15f;
    }

    protected override void Spawn()
    {
        Item pickupPrefab = PickupPrefabs[Random.Range(0, PickupPrefabs.Length)];
        float x = Random.Range(-2.5f, 2.5f);

        Instantiate(pickupPrefab, transform.position + (Vector3.right * x), Quaternion.identity, transform);
    }
}
