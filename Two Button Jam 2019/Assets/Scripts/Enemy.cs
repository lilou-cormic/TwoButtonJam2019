using System.Collections;
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
    private GameObject ShotPrefab = null;

    [SerializeField]
    private int _Points = 1;
    public int Points => _Points;

    private float _shootCoolDown = 0.5f;
    private float _coolDownTimer = 0f;

    [SerializeField]
    private LayerMask PlayerLayer = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
            Player.HitPlayer(1);

        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        _coolDownTimer -= Time.deltaTime;

        if (_coolDownTimer > 0)
            return;

        var player = Physics2D.OverlapCapsule(transform.position, new Vector2(1.5f, 11f), CapsuleDirection2D.Vertical, 0, PlayerLayer);

        if (player != null)
        {
            Instantiate(ShotPrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();

            _coolDownTimer = _shootCoolDown;
        }
    }
}
