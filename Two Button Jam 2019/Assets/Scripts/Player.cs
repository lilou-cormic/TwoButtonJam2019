using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour
{
    private static Player _instance = null;

    private SpriteRenderer SpriteRenderer;

    private Invinsibility Invinsibility;

    [SerializeField]
    private SpriteRenderer DamageSpriteRenderer = null;

    [SerializeField]
    private Sprite DamageSprite1 = null;

    [SerializeField]
    private Sprite DamageSprite2 = null;

    [SerializeField]
    private Sprite DamageSprite3 = null;

    [SerializeField]
    private GameObject Shield = null;

    [SerializeField]
    private GameObject ShieldPanel = null;

    [SerializeField]
    private TextMeshProUGUI ShieldCountDownText = null;

    [SerializeField]
    private AudioClip ShieldDownSound = null;

    [SerializeField]
    private GameObject WeapondPanel = null;
    [SerializeField]
    private TextMeshProUGUI WeaponCountDownText = null;

    public bool IsMoving { get; private set; } = true;
    public bool IsShooting { get; private set; } = false;

    private float _speed = 2.5f;
    private int _direction = 1;

    private float _button1Timer = 0;
    private float _stopThreshold = 0.1f;

    private float _button2Timer = 0;
    private float _chargingThreshold = 0.1f;
    private float _bigShotThreshold = 0.5f;

    private float _shootCoolDown = 1f;
    private float _coolDownTimer = 0f;

    private int _maxShots = 3;
    private int _shotsLeft = 3;

    private bool _isShieldUp = false;
    private float _shieldTime = 8f;
    private float _shieldTimeLeft = 0f;

    [SerializeField]
    private GameObject NormalShotPrefab = null;
    [SerializeField]
    private GameObject BigShotPrefab = null;

    private int _maxHitPoints = 4;
    private int HitPoints { get; set; } = 4;

    private void Awake()
    {
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        Invinsibility = GetComponentInChildren<Invinsibility>();
    }

    private void Start()
    {
        _instance = this;

        _direction = 1;

        _button1Timer = 0;
        _button2Timer = 0;

        _isShieldUp = false;
        _shieldTimeLeft = 0f;
        Shield.SetActive(false);
        ShieldPanel.SetActive(false);
        ShieldCountDownText.text = "0.0";

        WeapondPanel.SetActive(false);
        WeaponCountDownText.text = "0.0";

        HitPoints = _maxHitPoints;
        SetDamageSprite();

        _shotsLeft = _maxShots;
        _coolDownTimer = 0;
    }

    private void ChangeDirection()
    {
        _direction *= -1;
    }

    private void Update()
    {
        ManageButton1();

        ManageButton2();

        ManageEdges();

        ManageShield();
    }

    #region ManageButton1
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
    #endregion

    #region ManageButton2
    private void ManageButton2()
    {
        _coolDownTimer -= Time.deltaTime;

        if (_coolDownTimer > 0 && !IsShooting)
            return;

        if (!IsShooting)
            _shotsLeft = _maxShots;

        if (Input.GetButtonDown("Button2"))
        {
            _button2Timer = 0;
        }
        else if (Input.GetButton("Button2"))
        {
            _button2Timer += Time.deltaTime;

            if (_button2Timer > _bigShotThreshold)
                SpriteRenderer.color = Color.green;
            else if (_button2Timer > _chargingThreshold)
                SpriteRenderer.color = Color.yellow;
        }
        else if (Input.GetButtonUp("Button2"))
        {
            SpriteRenderer.color = Color.white;

            IsShooting = true;

            bool isFirstShot = _shotsLeft == _maxShots;

            if (_button2Timer > _bigShotThreshold)
                ShootBigShot();
            else
                ShootNormal();

            _button2Timer = 0;

            if (isFirstShot)
                _coolDownTimer = _shootCoolDown;
        }
    }
    #endregion

    #region ManageEdges
    private void ManageEdges()
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
    #endregion

    #region ManageShield
    private void ManageShield()
    {
        if (_shieldTimeLeft > 0)
        {
            ShieldCountDownText.text = _shieldTimeLeft.ToString("0.0");

            _shieldTimeLeft -= Time.deltaTime;

            if (_shieldTimeLeft <= 0)
                DropShield();
        }
    }
    #endregion

    private void ShootNormal()
    {
        if (_shotsLeft > 0)
        {
            Shoot(NormalShotPrefab);
            _shotsLeft--;
        }

        IsShooting = _shotsLeft > 0;
    }

    private void ShootBigShot()
    {
        Shoot(BigShotPrefab);

        IsShooting = false;
    }

    private void Shoot(GameObject shotPrefab)
    {
        Instantiate(shotPrefab, transform.position, Quaternion.identity);
    }

    private void FixedUpdate()
    {
        if (IsMoving)
            transform.position += Vector3.right * _direction * Time.deltaTime * _speed;
    }

    private void Hit(int damage)
    {
        if (_isShieldUp)
        {
            DropShield();
            return;
        }

        HitPoints -= damage;

        if (HitPoints <= 0)
        {
            ScoreManager.SetHighScore();
            SceneManager.LoadScene("GameOver");
            return;
        }

        SetDamageSprite();

        Invinsibility.MakeInvinsible();
    }

    private void Heal(int health)
    {
        if (HitPoints >= _maxHitPoints)
            return;

        HitPoints += health;

        SetDamageSprite();
    }

    private void LiftShield()
    {
        _isShieldUp = true;

        _shieldTimeLeft = _shieldTime;

        Shield.SetActive(true);
        ShieldCountDownText.text = _shieldTimeLeft.ToString("0.0");
        ShieldPanel.SetActive(true);
    }

    private void DropShield()
    {
        SoundPlayer.Play(ShieldDownSound);

        _isShieldUp = false;

        _shieldTimeLeft = 0;

        Shield.SetActive(false);
        ShieldCountDownText.text = "0.0";
        ShieldPanel.SetActive(false);
    }

    private void SetDamageSprite()
    {
        switch (HitPoints)
        {
            case 1:
                DamageSpriteRenderer.sprite = DamageSprite3;
                break;

            case 2:
                DamageSpriteRenderer.sprite = DamageSprite2;
                break;

            case 3:
                DamageSpriteRenderer.sprite = DamageSprite1;
                break;

            default:
                DamageSpriteRenderer.sprite = null;
                break;
        }
    }

    public static void HitPlayer(int damage)
    {
        _instance.Hit(damage);
    }

    public static void HealPlayer(int health)
    {
        _instance.Heal(health);
    }

    public static void ShieldPlayer()
    {
        _instance.LiftShield();
    }
}
