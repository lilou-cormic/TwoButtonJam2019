using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AutoMovement : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField]
    public Vector3 MovementVector = Vector3.down;

    public bool UseGameSpeed = false;

    private int _direction = 1;

    [SerializeField]
    private float Limit = 2.7f;

    private float _limitLeft = -2.7f;
    private float _limitRight = 2.7f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _limitLeft = Mathf.Max(_limitLeft, transform.position.x - Limit);
        _limitRight = Mathf.Min(_limitLeft + (Limit * 2), _limitRight);
        _limitLeft = Mathf.Min(_limitLeft, _limitRight - (Limit * 2));
    }

    private void Update()
    {
        if (transform.position.x <= _limitLeft)
        {
            _direction = 1;
            return;
        }

        if (transform.position.x >= _limitRight)
        {
            _direction = -1;
            return;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(MovementVector.x * _direction, MovementVector.y, MovementVector.z);

        if (UseGameSpeed)
            rb.velocity *= GameOptions.GameSpeed;
    }
}
