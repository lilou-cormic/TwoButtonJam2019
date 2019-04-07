using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyShot : Shot
{
    [SerializeField]
    private AudioClip ExplosionSound = null;

    private void Update()
    {
        if (transform.position.y >= 10)
        {
            Destroy(gameObject);
            return;
        }
    }

    protected override void ManageTriggerEnter2D(Collider2D collision)
    {
        SoundPlayer.Play(ExplosionSound);

        if (collision.gameObject.CompareTag("Player"))
            Player.TakeDamage(1);
    }
}
