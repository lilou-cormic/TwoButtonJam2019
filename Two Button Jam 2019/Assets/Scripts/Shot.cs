using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shot : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    private float Speed = 9f;

    [SerializeField]
    private bool Rampage = false;

    [SerializeField]
    private AudioClip ShootSound = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.up * Speed;
    }
    private void Start()
    {
        SoundPlayer.Play(ShootSound);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ManageTriggerEnter2D(collision);

        if (!Rampage)
        {
            Destroy(gameObject);
            return;
        }
    }

    protected abstract void ManageTriggerEnter2D(Collider2D collision);
}
