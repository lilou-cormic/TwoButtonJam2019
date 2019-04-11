using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : Shot
{
    [SerializeField]
    private UIPoints PointsDisplayPrefab = null;

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
        if (transform.position.y >= 9)
            return;

        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy != null && !enemy.IsDead)
        {
            enemy.Die();

            int pts = ScoreManager.AddPoints(enemy.Points);
            var uIPoints = Instantiate(PointsDisplayPrefab, collision.transform.position + Vector3.up * 0.5f, Quaternion.identity);
            uIPoints.SetPointsText(pts);

            ScoreManager.IncrementMultiplier();
        }
    }
}
