using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    private float Speed = 5f;

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

    private void Update()
    {
        if (transform.position.y >= 10)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ScoreManager.AddPoints(1);

        if (!Rampage)
        {
            Destroy(gameObject);
            return;
        }
    }
}
