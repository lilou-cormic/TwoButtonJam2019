using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    private float _countDown = 1.5f;

    protected float MinCountDown { get; set; } = 1.5f;
    protected float MaxCountDown { get; set; } = 5f;

    private void Awake()
    {
        _countDown = MinCountDown;
    }

    private void FixedUpdate()
    {
        _countDown -= Time.fixedDeltaTime;

        if (_countDown <= 0)
        {
            _countDown = Random.Range(MinCountDown, MaxCountDown);

            Spawn();
        }

        AjustCoolDown();
    }

    protected virtual void AjustCoolDown() { }

    protected abstract void Spawn();
}
