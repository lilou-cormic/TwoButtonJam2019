using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AutoMovement : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField]
    public Vector3 MovementVector = Vector3.down;

    private int _direction = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (transform.position.x <= -2.7)
        {
            _direction = 1;
            return;
        }

        if (transform.position.x >= 2.7)
        {
            _direction = -1;
            return;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(MovementVector.x * _direction, MovementVector.y, MovementVector.z);
    }
}
