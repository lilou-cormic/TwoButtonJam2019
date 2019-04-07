﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    private AudioClip ExplosionSound = null;

    [SerializeField]
    private GameObject ExplosionSmoke = null;

    [SerializeField]
    private float Speed = 2f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.down * Speed;
    }

    private void Update()
    {
        if (transform.position.y <= -2)
        {
            ScoreManager.ResetScoreMultiplier();

            Destroy(gameObject);
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SoundPlayer.Play(ExplosionSound);

        var smoke = Instantiate(ExplosionSmoke, transform.position, Quaternion.identity);
        Destroy(smoke, 0.5f);

        if (collision.gameObject.CompareTag("Player"))
        {
            ScoreManager.SetHighScore();
            SceneManager.LoadScene("GameOver");
            return;
        }

        Destroy(gameObject);
    }
}
