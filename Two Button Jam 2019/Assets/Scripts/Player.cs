using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer SpriteRenderer;

    public float Speed = 2.5f;

    public bool IsMoving { get; private set; } = true;

    private int _direction = 1;

    private float _button1Timer = 0;
    private float _stopThreshold = 0.1f;

    private float _button2Timer = 0;
    private float _chargingThreshold = 0.1f;
    private float _bigShotThreshold = 0.5f;

    [SerializeField]
    private GameObject NormalShotPrefab = null;
    [SerializeField]
    private GameObject BigShotPrefab = null;

    private void Awake()
    {
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void ChangeDirection()
    {
        _direction *= -1;
    }

    private void Update()
    {
        ManageButton1();

        ManageButton2();

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

    private void ManageButton1()
    {
        if (Input.GetButtonDown("Button1"))
        {
            _button1Timer = 0;
            IsMoving = false;
        }
        else if (Input.GetButton("Button1"))
        {
            _button1Timer += Time.deltaTime;
        }
        else if (Input.GetButtonUp("Button1"))
        {
            IsMoving = true;

            if (_button1Timer < _stopThreshold)
                ChangeDirection();

            _button1Timer = 0;
        }
    }

    private void ManageButton2()
    {
        if (Input.GetButtonDown("Button2"))
        {
            _button2Timer = 0;
        }
        else if (Input.GetButton("Button2"))
        {
            _button2Timer += Time.deltaTime;

            if (_button2Timer > _bigShotThreshold)
                SpriteRenderer.color = Color.red;
            else if (_button2Timer > _chargingThreshold)
                SpriteRenderer.color = Color.magenta;
        }
        else if (Input.GetButtonUp("Button2"))
        {
            SpriteRenderer.color = Color.white;

            if (_button2Timer > _bigShotThreshold)
                ShootBigShot();
            else
                ShootNormal();

            _button2Timer = 0;
        }
    }

    private void ShootNormal()
    {
        Shoot(NormalShotPrefab);
    }

    private void ShootBigShot()
    {
        Shoot(BigShotPrefab);
    }

    private void Shoot(GameObject shotPrefab)
    {
        Instantiate(shotPrefab, transform.position, Quaternion.identity);
    }

    private void LateUpdate()
    {
        if (IsMoving)
            transform.position += Vector3.right * _direction * Time.deltaTime * Speed;
    }
}
